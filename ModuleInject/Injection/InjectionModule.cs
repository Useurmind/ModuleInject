using ModuleInject.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces;
using ModuleInject.Injection.Hooks;
using ModuleInject.Interfaces.Injection;
using System.Linq.Expressions;
using ModuleInject.Common.Utility;
using ModuleInject.Utility;
using ModuleInject.Common.Exceptions;
using System.Runtime.CompilerServices;
using ModuleInject.Interfaces.Hooks;

namespace ModuleInject.Injection
{
    public interface IInjectionModule
    {
        void RegisterInjectionRegister(IInjectionRegister injectionRegister);
    }

    public class InjectionModule<TModule> : Module, IInjectionModule
        where TModule : InjectionModule<TModule>
    {
        private ISet<IInjectionRegister> injectionRegisters;
        private IDictionary<string, IInjectionRegister> namedInjectionRegisters;

        private IRegistry usedRegistry;
        private IEnumerable<IRegistrationHook> allRegistrationHooks;

        public InjectionModule()
        {
            this.injectionRegisters = new HashSet<IInjectionRegister>();
            this.namedInjectionRegisters = new Dictionary<string, IInjectionRegister>();
        }

        protected IDisposeStrategyContext<TModule, TIComponent> Factory<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
        {
            return SourceOf<TIComponent>(componentMember)
                .InstantiateWith(new FactoryInstantiationStrategy<TIComponent>());
        }

        protected IDisposeStrategyContext<TModule, TIComponent> SingleInstance<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
        {
            return SourceOf<TIComponent>(componentMember)
                .InstantiateWith(new SingleInstanceInstantiationStrategy<TIComponent>());
        }

        protected IDisposeStrategyContext<TModule, TIComponent> Factory2<TIComponent>([CallerMemberName]string componentName = null)
        {
            return SourceOf<TIComponent>(componentName)
                .InstantiateWith(new FactoryInstantiationStrategy<TIComponent>());
        }

        protected IDisposeStrategyContext<TModule, TIComponent> SingleInstance2<TIComponent>([CallerMemberName]string componentName = null)
        {
            return SourceOf<TIComponent>(componentName)
                .InstantiateWith(new SingleInstanceInstantiationStrategy<TIComponent>());
        }

        protected IDisposeStrategyContext<TModule, TIComponent> Factory<TIComponent>()
        {
            return SourceOf<TIComponent>()
                .InstantiateWith(new FactoryInstantiationStrategy<TIComponent>());
        }

        protected IDisposeStrategyContext<TModule, TIComponent> SingleInstance<TIComponent>()
        {
            return SourceOf<TIComponent>()
                .InstantiateWith(new SingleInstanceInstantiationStrategy<TIComponent>());
        }

        protected IInstantiationStrategyContext<TModule, TIComponent> SourceOf<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
        {
            var componentName = GetComponentName(componentMember);

            return SourceOf<TIComponent>(componentName.Name);
        }

        protected IInstantiationStrategyContext<TModule, TIComponent> SourceOf<TIComponent>()
        {
            return SourceOf<TIComponent>((string)null);
        }

        protected IInstantiationStrategyContext<TModule, TIComponent> SourceOf<TIComponent>(string componentName)
        {
            IInjectionRegister injectionRegister = new InjectionRegister(componentName, typeof(TModule), typeof(TIComponent));

            injectionRegister.SetContext(this);

            return new StartContext<TModule, TIComponent>((TModule)this, injectionRegister);
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

                this.namedInjectionRegisters.Add(injectionRegister.ComponentName, injectionRegister);
            }
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
        protected TComponent GetFactory<TComponent>(
            [CallerMemberName]string componentName = null)
            where TComponent : new()
        {
            return GetFactory(m => new TComponent(), componentName);
        }

        protected TComponent GetFactory<TComponent>(
            Func<TModule, TComponent> construct,
            [CallerMemberName]string componentName = null)
        {
            Action<IConstructionContext<TModule, TComponent>> registerComponent = cc =>
            {
                cc.Construct(construct);
            };
            return GetFactory(registerComponent, componentName);
        }

        protected TIComponent GetFactory<TIComponent>(
            Action<IConstructionContext<TModule, TIComponent>> registerComponent,
            [CallerMemberName]string componentName = null)
        {
            return Get<TIComponent>(ctx =>
            {
                var constructCtx = ctx.InstantiateWith(new FactoryInstantiationStrategy<TIComponent>());
                registerComponent(constructCtx);
            }, componentName);
        }

        protected TComponent GetSingleInstance<TComponent>(
            [CallerMemberName]string componentName = null)
            where TComponent : new()
        {
            return GetSingleInstance(m => new TComponent(), componentName);
        }

        protected TComponent GetSingleInstance<TComponent>(
            Func<TModule, TComponent> construct,
            [CallerMemberName]string componentName = null)
        {
            Action<IConstructionContext<TModule, TComponent>> registerComponent = cc =>
            {
                cc.Construct(construct);
            };
            return GetSingleInstance(registerComponent, componentName);
        }

        protected TIComponent GetSingleInstance<TIComponent>(
            Action<IConstructionContext<TModule, TIComponent>> registerComponent,
            [CallerMemberName]string componentName = null)
        {
            return Get<TIComponent>(ctx =>
            {
                var constructCtx = ctx.InstantiateWith(new SingleInstanceInstantiationStrategy<TIComponent>());
                registerComponent(constructCtx);
            }, componentName);
        }

        protected TIComponent Get<TIComponent>(
            Action<IInstantiationStrategyContext<TModule, TIComponent>> registerComponent,
            [CallerMemberName]string componentName = null)
        {
            if (!this.namedInjectionRegisters.ContainsKey(componentName))
            {
                var constructionContext = this.SourceOf<TIComponent>(componentName);
                registerComponent(constructionContext);
            }

            return (TIComponent)Get(componentName);
        }

        protected TIComponent Get<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
        {
            CommonFunctions.CheckNullArgument("componentMember", componentMember);

            var componentName = GetComponentName(componentMember);

            return (TIComponent)Get(componentName.Name);
        }


        protected TIComponent Get<TIComponent>([CallerMemberName]string componentName = null)
        {
            return (TIComponent)Get(componentName);
        }

        protected object Get(string componentName)
        {
            // this must be available during resolution now, because of lambda expression
            if (!this.IsResolving && !this.IsResolved)
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_CreateInstanceBeforeResolve, componentName);
            }

            if (!this.namedInjectionRegisters.ContainsKey(componentName))
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_ComponentNotRegistered, componentName);
            }

            return this.namedInjectionRegisters[componentName].GetInstance();
        }

        private System.Reflection.MemberInfo GetComponentName<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
        {
            Expression body = componentMember.Body;
            if (body.NodeType == ExpressionType.MemberAccess)
            {
                var member = (MemberExpression)body;
                return member.Member;
            }
            else if (body.NodeType == ExpressionType.Call)
            {
                var method = (MethodCallExpression)body;
                return method.Method;
            }

            throw new ArgumentException("Expression for component not correct.");
        }

        private void TryAddModuleResolveHooks()
        {
            foreach (var injectionRegister in injectionRegisters)
            {
                TryAddModuleResolveHook(injectionRegister);
            }
        }

        void TryAddModuleResolveHook(IInjectionRegister injectionRegister)
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
    }
}
