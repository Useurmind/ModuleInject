using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Hooks;
using ModuleInject.Interfaces.Injection;
using ModuleInject.Modularity;

namespace ModuleInject.Injection
{
    /// <summary>
    /// This is internal functionality of the <see cref="InjectionModule{TModule}"/> class.
    /// Do NOT use this to derive modules.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public abstract class InjectionModuleCore<TModule> : Module
    {
        private HashSet<IInjectionRegister> injectionRegisters;
        
        private DoubleKeyDictionary<Type, string, IInjectionRegister> namedInjectionRegisters;

        private IRegistry usedRegistry;
        private IEnumerable<IRegistrationHook> allRegistrationHooks;

        internal InjectionModuleCore()
        {
            this.injectionRegisters = new HashSet<IInjectionRegister>();
            this.namedInjectionRegisters = new DoubleKeyDictionary<Type, string, IInjectionRegister>();
        }


        /// <inheritdoc />
        protected override void OnRegistryResolved(IRegistry usedRegistry)
        {
            this.usedRegistry = usedRegistry;

            var registrationHooksFromRegistry = this.usedRegistry.GetRegistrationHooks().Where(h => h.AppliesToModule(this));
            var registrationHooksFromModule = this.RegistrationHooks;
            allRegistrationHooks = registrationHooksFromModule.Union(registrationHooksFromRegistry);

            TryAddRegistrationHooks();
            TryAddModuleResolveHooks();
        }

        /// <summary>
        /// Function that can be overwritten to hook into the resolution process.
        /// Called once a component is resolved.
        /// </summary>
        /// <param name="context">A context that describes what has been resolved.</param>
        protected virtual void OnComponentResolved(ObjectResolvedContext context)
        {
        }

        /// <summary>
        /// This is called to register <see cref="IInjectionRegister"/> instances with the module.
        /// Automatically called for everything created through the (Get)SourceOf, (Get)Factory, and (Get)SingleInstance sort of methods.
        /// </summary>
        /// <param name="injectionRegister">The injection register to register.</param>
        public void RegisterInjectionRegister(IInjectionRegister injectionRegister)
        {
            injectionRegister.OnResolve(context => this.OnComponentResolved(context));
            this.injectionRegisters.Add(injectionRegister);
            if (!string.IsNullOrEmpty(injectionRegister.ComponentName))
            {
                if (this.IsResolved)
                {
                    this.TryAddRegistrationHooks(injectionRegister);
                    this.TryAddModuleResolveHook(injectionRegister);
                }

                this.namedInjectionRegisters.Add(injectionRegister.ComponentInterface, injectionRegister.ComponentName, injectionRegister);
            }
        }

        /// <summary>
        /// Dispose all registered injection registers.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.IsDisposed)
                {
                    foreach (var injectionRegister in this.injectionRegisters)
                    {
                        injectionRegister.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }

        private void TryAddRegistrationHooks()
        {
            foreach (var injectionRegister in this.injectionRegisters)
            {
                TryAddRegistrationHooks(injectionRegister);
            }
        }

        private void TryAddRegistrationHooks(IInjectionRegister injectionRegister)
        {
            if (!allRegistrationHooks.Any())
            {
                return;
            }

            foreach (var registrationHook in allRegistrationHooks)
            {
                if (registrationHook.AppliesToRegistration(injectionRegister))
                {
                    registrationHook.Execute(injectionRegister);
                }
            }
        }
        private void TryAddModuleResolveHooks()
        {
            foreach (var injectionRegister in injectionRegisters)
            {
                TryAddModuleResolveHook(injectionRegister);
            }
        }

        private void TryAddModuleResolveHook(IInjectionRegister injectionRegister)
        {
            var moduleTypeName = typeof(IModule).Name;
            var isModule = injectionRegister.ComponentType.GetInterface(moduleTypeName) != null;
            if (isModule)
            {
                injectionRegister.OnResolve(this.OnRegisteredModuleResolved);
            }
        }

        private void OnRegisteredModuleResolved(ObjectResolvedContext context)
        {
            var module = context.Instance as IModule;
            if (module != null)
            {
                module.Resolve(this.usedRegistry);
            }
        }

        /// <summary>
        /// Check if a registration is already present for the given type and component name.
        /// </summary>
        /// <typeparam name="TIComponent">The type of the component to check.</typeparam>
        /// <param name="componentName">The name of the component to check.</param>
        /// <returns>True if yes.</returns>
        protected bool HasRegistration<TIComponent>([CallerMemberName]string componentName = null)
        {
            return HasRegistration(typeof(TIComponent), componentName);
        }

        /// <summary>
        /// Check if a registration is already present for the given type and component name.
        /// </summary>
        /// <param name="componentInterface">The type of the component to check.</param>
        /// <param name="componentName">The name of the component to check.</param>
        /// <returns>True if yes.</returns>
        protected bool HasRegistration(Type componentInterface, [CallerMemberName]string componentName = null)
        {
            return this.namedInjectionRegisters.Contains(componentInterface, componentName);
        }

        /// <summary>
        /// Retrieve a named component from the module which is already registered.
        /// IMPORTANT: It must already be registered, e.g. by calling a form of SourceOf or at least once GetSourceOf for this component.
        /// </summary>
        /// <typeparam name="TIComponent">The type of the component to retrieve.</typeparam>
        /// <param name="componentName">The name of the component to retrieve.</param>
        /// <returns>An instance of the component.</returns>
        protected TIComponent Get<TIComponent>([CallerMemberName]string componentName = null)
        {
            return (TIComponent)Get(typeof(TIComponent), componentName);
        }

        /// <summary>
        /// Retrieve a named component from the module which is already registered.
        /// IMPORTANT: It must already be registered, e.g. by calling a form of SourceOf or at least once GetSourceOf for this component.
        /// </summary>
        /// <param name="componentInterface">The type of the component to retrieve.</param>
        /// <param name="componentName">The name of the component to retrieve, can be null but then type must be unique.</param>
        /// <returns>An instance of the component.</returns>
        protected object Get(Type componentInterface, string componentName = null)
        {
            // this must be available during resolution now, because of lambda expression
            if (!this.IsResolving && !this.IsResolved)
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_CreateInstanceBeforeResolve, componentName);
            }

            if(string.IsNullOrEmpty(componentName))
            {
                var registeredUnderType = this.namedInjectionRegisters.GetAll(componentInterface);
                if(registeredUnderType.Count() > 1)
                {
                    ExceptionHelper.ThrowFormatException(Errors.InjectionModule_MultipleRegistrationsUnderInterface, componentInterface.Name, typeof(TModule).Name);
                }
                return registeredUnderType.First().GetInstance();
            }

            IInjectionRegister injectionRegister = null;
            if (!this.namedInjectionRegisters.TryGetValue(componentInterface, componentName, out injectionRegister))
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_ComponentNotRegistered, componentName);
            }

            return injectionRegister.GetInstance();
        }
    }
}
