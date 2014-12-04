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

    public static class ComponentInterfaceRegistrationContextExtensions
    {
        #region component interface
        
        public static IInterfaceRegistrationContext<IComponent, IModule, TModule>
             Inject<IComponent, IModule, TModule>(
             this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
             Expression<Action<IComponent, TModule>> methodCallExpression)
             where TModule : IModule
             where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.Inject(methodCallExpression);

            return component;
        }

        public static IInterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency>
            Inject<IComponent, IModule, TModule, TDependency>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
            TDependency value)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            var valueContext = contextImpl.Context.Inject(value, typeof(TDependency));
            return new InterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency>(contextImpl, valueContext);
        }

        public static IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency>
            Inject<IComponent, IModule, TModule, TDependency>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency>> dependencySourceExpression)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            var dependencyContext = contextImpl.Context.InjectSource((LambdaExpression)dependencySourceExpression);

            return new InterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency>(
                contextImpl, dependencyContext);
        }

        public static IInterfaceRegistrationContext<IComponent, IModule, TModule>
            AddInjector<IComponent, IModule, TModule>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
            IInterfaceInjector<IComponent, IModule, TModule> injector)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injector", injector);

            injector.InjectInto(component);
            return component;
        }

        /// <summary>
        /// Add a behavior of the given type.
        /// </summary>
        /// <typeparam name="TBehaviour">The type of the behavior to add.</typeparam>
        /// <returns>The current context of the fluent API.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This API is by design statically typed")]
        public static IInterfaceRegistrationContext<IComponent, IModule, TModule> AddBehaviour<IComponent, IModule, TModule, TBehaviour>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
            TBehaviour behaviour)
            where TModule : IModule
            where IModule : IInjectionModule
            where TBehaviour : Microsoft.Practices.Unity.InterceptionExtension.IInterceptionBehavior
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.AddBehaviour(behaviour);

            return component;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule, TModule> AddCustomAction<IComponent, IModule, TModule>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
            Action<IComponent> customAction)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.AddCustomAction(customAction);

            return component;
        }

        private static InterfaceRegistrationContext<IComponent, IModule, TModule> GetContextImplementation<IComponent, IModule, TModule>(IInterfaceRegistrationContext<IComponent, IModule, TModule> component)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            return (InterfaceRegistrationContext<IComponent, IModule, TModule>)component;
        }

        #endregion
    }
}

