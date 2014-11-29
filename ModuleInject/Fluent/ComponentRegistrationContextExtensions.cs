using Microsoft.Practices.Unity;

using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Fluent
{
    using System.Diagnostics.CodeAnalysis;

    using ModuleInject.Common.Utility;
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public static class ComponentRegistrationContextExtensions
    {
        //public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
        //    ModifyDependencyBy<IComponent, TComponent, IModule, TModule, TDependency>(
        //    this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
        //    Expression<Func<TModule, TDependency>> dependencySourceExpression,
        //    Action<TDependency> modifyAction)
        //    where TComponent : IComponent
        //    where TModule : IModule
        //    where IModule : IInjectionModule
        //{
        //    CommonFunctions.CheckNullArgument("component", component);

        //    var contextImpl = GetContextImplementation(component);

        //    contextImpl.Context.ModifyDependencyBy(dependencySourceExpression, obj => modifyAction((TDependency)obj));
        //    return component;
        //}

        public static IValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            TDependency value)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            var valueContext = contextImpl.Context.Inject(value, typeof(TDependency));
            return new ValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(contextImpl, valueContext);
        }

        public static IDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency>> dependencySourceExpression)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            var dependencyContext = contextImpl.Context.Inject((Expression)dependencySourceExpression);

            return new DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(
                contextImpl, dependencyContext);
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
             Inject<IComponent, TComponent, IModule, TModule>(
             this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
             Expression<Action<TComponent, TModule>> methodCallExpression)
             where TComponent : IComponent
             where TModule : IModule
             where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.Inject(methodCallExpression);

            return component;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            IClassInjector<IComponent, TComponent, IModule, TModule> injector)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injector", injector);

            injector.InjectInto(component);
            return component;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            IInterfaceInjector<IComponent, IModule, TModule> injector)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);
            CommonFunctions.CheckNullArgument("injector", injector);

            var contextImpl = GetContextImplementation(component);

            var interfaceContext = new InterfaceRegistrationContext<IComponent, IModule, TModule>(contextImpl.Context);
            injector.InjectInto(interfaceContext);
            return component;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule, IComponentBase, IModuleBase>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            IInterfaceInjector<IComponentBase, IModuleBase> injector)
            where TComponent : IComponent, IComponentBase
            where TModule : IModule, IModuleBase
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);
            CommonFunctions.CheckNullArgument("injector", injector);

            var contextImpl = GetContextImplementation(component);

            var interfaceContext = new InterfaceRegistrationContext<IComponentBase, IModuleBase>(contextImpl.Context);
            injector.InjectInto(interfaceContext);
            return component;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddTypeInjection<IComponent, TComponent, IModule, TModule>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Action<IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> injectInto)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            ClassInjector<IComponent, TComponent, IModule, TModule> injector =
                new ClassInjector<IComponent, TComponent, IModule, TModule>(injectInto);

            component.AddInjector(injector);
            return component;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInterfaceInjection<IComponent, TComponent, IModule, TModule>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Action<IInterfaceRegistrationContext<IComponent, IModule, TModule>> injectInto)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            InterfaceInjector<IComponent, IModule, TModule> injector =
                new InterfaceInjector<IComponent, IModule, TModule>(injectInto);

            component.AddInjector(injector);
            return component;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInterfaceInjection<IComponent, TComponent, IModule, TModule, IComponentBase, IModuleBase>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Action<IInterfaceRegistrationContext<IComponentBase, IModuleBase>> injectInto)
            where TComponent : IComponent, IComponentBase
            where TModule : IModule, IModuleBase
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            InterfaceInjector<IComponentBase, IModuleBase> injector =
                new InterfaceInjector<IComponentBase, IModuleBase>(injectInto);

            component.AddInjector(injector);
            return component;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AlsoRegisterFor<IComponent, TComponent, IModule, TModule, IComponent2>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<TModule, IComponent2>> secondModuleProperty)
            where TComponent : IComponent
            where IComponent : IComponent2
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.AlsoRegisterFor(secondModuleProperty);

            return component;
        }

        /// <summary>
        /// Add a behavior of the given type.
        /// </summary>
        /// <typeparam name="TBehaviour">The type of the behavior to add.</typeparam>
        /// <returns>The current context of the fluent API.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This API is by design statically typed")]
        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> AddBehaviour<IComponent, TComponent, IModule, TModule, TBehaviour>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            TBehaviour behaviour)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
            where TBehaviour : Microsoft.Practices.Unity.InterceptionExtension.IInterceptionBehavior
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.AddBehaviour(behaviour);

            return component;
        }

        private static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> GetContextImplementation<IComponent, TComponent, IModule, TModule>(IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            return (ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>)component;
        }
    }
}

