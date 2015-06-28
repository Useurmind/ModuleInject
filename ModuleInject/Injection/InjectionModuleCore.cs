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
        protected override void OnRegistryResolved(IRegistry usedRegistry)
        {
            this.usedRegistry = usedRegistry;

            var registrationHooksFromRegistry = this.usedRegistry.GetRegistrationHooks().Where(h => h.AppliesToModule(this));
            var registrationHooksFromModule = this.RegistrationHooks;
            allRegistrationHooks = registrationHooksFromModule.Union(registrationHooksFromRegistry);

            TryAddRegistrationHooks();
            TryAddModuleResolveHooks();
        }

        protected virtual void OnComponentResolved(ObjectResolvedContext context)
        {
        }

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

        protected bool HasRegistration<TIComponent>([CallerMemberName]string componentName = null)
        {
            return HasRegistration(typeof(TIComponent), componentName);
        }

        protected bool HasRegistration(Type componentInterface, [CallerMemberName]string componentName = null)
        {
            return this.namedInjectionRegisters.Contains(componentInterface, componentName);
        }

        /// <summary>
        /// Retrieve a named component from the module which is already registered.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent Get<TIComponent>([CallerMemberName]string componentName = null)
        {
            return (TIComponent)Get(typeof(TIComponent), componentName);
        }

        protected object Get(Type componentInterface, [CallerMemberName]string componentName = null)
        {
            // TODO: make null component name valid for cases where only one registration for a type is given

            // this must be available during resolution now, because of lambda expression
            if (!this.IsResolving && !this.IsResolved)
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_CreateInstanceBeforeResolve, componentName);
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
