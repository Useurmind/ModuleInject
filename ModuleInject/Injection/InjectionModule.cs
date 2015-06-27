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
    /// <summary>
    /// Base class for modules performing dependency injection.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public class InjectionModule<TModule> : InjectionModuleCore<TModule>, IInjectionModule
        where TModule : InjectionModule<TModule>
    {
        /// <summary>
        /// Register a nameless component with the module and preset instantiation strategy to factory.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <returns></returns>
        protected IDisposeStrategyContext<TModule, TIComponent> CreateFactory<TIComponent>()
        {
            return CreateSourceOf<TIComponent>()
                .InstantiateWith(new FactoryInstantiationStrategy<TIComponent>());
        }

        /// <summary>
        /// Register a nameless component with the module and preset instantiation strategy to single instance.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <returns></returns>
        protected IDisposeStrategyContext<TModule, TIComponent> CreateSingleInstance<TIComponent>()
        {
            return CreateSourceOf<TIComponent>()
                .InstantiateWith(new SingleInstanceInstantiationStrategy<TIComponent>());
        }

        /// <summary>
        /// Register a nameless component with the module (no presets).
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <returns></returns>
        protected IInstantiationStrategyContext<TModule, TIComponent> CreateSourceOf<TIComponent>()
        {
            return SourceOf<TIComponent>((string)null);
        }

        /// <summary>
        /// Register a named component with the module and preset instantiation strategy to factory.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="componentMember"></param>
        /// <returns></returns>
        protected IDisposeStrategyContext<TModule, TIComponent> Factory<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
        {
            return SourceOf<TIComponent>(componentMember)
                .InstantiateWith(new FactoryInstantiationStrategy<TIComponent>());
        }

        /// <summary>
        /// Register a named component with the module and preset instantiation strategy to single instance.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="componentMember"></param>
        /// <returns></returns>
        protected IDisposeStrategyContext<TModule, TIComponent> SingleInstance<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
        {
            return SourceOf<TIComponent>(componentMember)
                .InstantiateWith(new SingleInstanceInstantiationStrategy<TIComponent>());
        }

        /// <summary>
        /// Register a named component with the module and preset instantiation strategy to factory.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected IDisposeStrategyContext<TModule, TIComponent> Factory<TIComponent>([CallerMemberName]string componentName = null)
        {
            return SourceOf<TIComponent>(componentName)
                .InstantiateWith(new FactoryInstantiationStrategy<TIComponent>());
        }

        /// <summary>
        /// Register a named component with the module and preset instantiation strategy to single instance.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected IDisposeStrategyContext<TModule, TIComponent> SingleInstance<TIComponent>([CallerMemberName]string componentName = null)
        {
            return SourceOf<TIComponent>(componentName)
                .InstantiateWith(new SingleInstanceInstantiationStrategy<TIComponent>());
        }

        /// <summary>
        /// Register a named component with the module (no presets).
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="componentMember"></param>
        /// <returns></returns>
        protected IInstantiationStrategyContext<TModule, TIComponent> SourceOf<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
        {
            var componentName = GetComponentName(componentMember);

            return SourceOf<TIComponent>(componentName.Name);
        }


        /// <summary>
        /// Register a named component with the module (no presets).
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected IInstantiationStrategyContext<TModule, TIComponent> SourceOf<TIComponent>(string componentName)
        {
            IInjectionRegister injectionRegister = new InjectionRegister(componentName, typeof(TModule), typeof(TIComponent));

            injectionRegister.SetContext(this);

            return new StartContext<TModule, TIComponent>((TModule)this, injectionRegister);
        }



        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to factory.
        /// Shortcut for simple construction without interface.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TComponent GetFactory<TComponent>(
            [CallerMemberName]string componentName = null)
            where TComponent : new()
        {
            return GetFactoryConstructed(m => new TComponent(), componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to factory.
        /// Shortcut for simple construction with interface and type.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent GetFactory<TIComponent, TComponent>(
            [CallerMemberName]string componentName = null)
            where TComponent : TIComponent, new()
        {
            return GetFactoryConstructed<TIComponent>(m => new TComponent(), componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to factory.
        /// Shortcut for constructor / property initializer injection.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="construct"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent GetFactoryConstructed<TIComponent>(
            Func<TModule, TIComponent> construct,
            [CallerMemberName]string componentName = null)
        {
            Action<IConstructionContext<TModule, TIComponent>> registerComponent = cc =>
            {
                cc.Construct(construct);
            };
            return GetFactory(registerComponent, componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to factory.
        /// Shortcut for fast method injection.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="construct"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent GetFactoryInjected<TIComponent, TComponent>(
            Action<TModule, TComponent> inject,
            [CallerMemberName]string componentName = null)
            where TComponent : TIComponent, new()
        {
            return GetFactory<TIComponent>(cc =>
            {
                cc.Construct<TComponent>()
                  .Inject(inject);
            }, componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to factory.
        /// Shortcut for registration with full fluent API.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="registerComponent"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent GetFactory<TIComponent>(
            Action<IConstructionContext<TModule, TIComponent>> registerComponent,
            [CallerMemberName]string componentName = null)
        {
            return GetSourceOf<TIComponent>(ctx =>
            {
                var constructCtx = ctx.InstantiateWith(new FactoryInstantiationStrategy<TIComponent>());
                registerComponent(constructCtx);
            }, componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to single instance.
        /// Shortcut for simple construction without interface.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TComponent GetSingleInstance<TComponent>([CallerMemberName]string componentName = null)
            where TComponent : new()
        {
            return GetSingleInstanceConstructed(m => new TComponent(), componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to single instance.
        /// Shortcut for simple construction with interface and type.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent GetSingleInstance<TIComponent, TComponent>(
            [CallerMemberName]string componentName = null)
            where TComponent : TIComponent, new()
        {
            return GetSingleInstanceConstructed<TIComponent>(m => new TComponent(), componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to single instance.
        /// Shortcut for constructor/property initializer injection.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="construct"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent GetSingleInstanceConstructed<TIComponent>(
            Func<TModule, TIComponent> construct,
            [CallerMemberName]string componentName = null)
        {
            Action<IConstructionContext<TModule, TIComponent>> registerComponent = cc =>
            {
                cc.Construct(construct);
            };
            return GetSingleInstance(registerComponent, componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to single instance.
        /// Shortcut for fast method injection.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="inject"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent GetSingleInstanceInjected<TIComponent, TComponent>(
            Action<TModule, TComponent> inject,
            [CallerMemberName]string componentName = null)
            where TComponent : TIComponent, new()
        {
            return GetSingleInstance<TIComponent>(cc =>
            {
                cc.Construct<TComponent>()
                  .Inject(inject);
            }, componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// Instantiation strategy preset to single instance.
        /// Shortcut for full construction with fluent language.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="registerComponent"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent GetSingleInstance<TIComponent>(
            Action<IConstructionContext<TModule, TIComponent>> registerComponent,
            [CallerMemberName]string componentName = null)
        {
            return GetSourceOf<TIComponent>(ctx =>
            {
                var constructCtx = ctx.InstantiateWith(new SingleInstanceInstantiationStrategy<TIComponent>());
                registerComponent(constructCtx);
            }, componentName);
        }

        /// <summary>
        /// Register and retrieve a named component with/from the module.
        /// No presets, full fluent API workflow.
        /// Most general getter function with complete fluent registration workflow.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="registerComponent"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent GetSourceOf<TIComponent>(
            Action<IInstantiationStrategyContext<TModule, TIComponent>> registerComponent,
            [CallerMemberName]string componentName = null)
        {
            if (!this.HasRegistration(componentName))
            {
                var constructionContext = this.SourceOf<TIComponent>(componentName);
                registerComponent(constructionContext);
            }

            return (TIComponent)Get(componentName);
        }

        /// <summary>
        /// Retrieve a named component from the module which is already registered.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="componentMember"></param>
        /// <returns></returns>
        protected TIComponent Get<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
        {
            CommonFunctions.CheckNullArgument("componentMember", componentMember);

            var componentName = GetComponentName(componentMember);

            return (TIComponent)Get(componentName.Name);
        }

        /// <summary>
        /// Retrieve a named component from the module which is already registered.
        /// </summary>
        /// <typeparam name="TIComponent"></typeparam>
        /// <param name="componentName"></param>
        /// <returns></returns>
        protected TIComponent Get<TIComponent>([CallerMemberName]string componentName = null)
        {
            return (TIComponent)Get(componentName);
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
    }
}
