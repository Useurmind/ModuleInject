using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public static class ComponentAndModuleInterfaceRegistrationContextExtensions{

        #region component and module interface

        public static IInterfaceRegistrationContext<IComponent, IModule>
             Inject<IComponent, IModule>(
             this IInterfaceRegistrationContext<IComponent, IModule> component,
             Expression<Action<IComponent>> methodCallExpression)
             where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.Inject(methodCallExpression);

            return component;
        }

        public static IInterfaceValueInjectionContext<IComponent, IModule, TDependency>
            Inject<IComponent, IModule, TDependency>(
            this IInterfaceRegistrationContext<IComponent, IModule> component,
            TDependency value)
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            var valueContext = contextImpl.Context.Inject(value, typeof(TDependency));
            return new InterfaceValueInjectionContext<IComponent, IModule, TDependency>(contextImpl, valueContext);
        }

        public static IInterfaceDependencyInjectionContext<IComponent, IModule, TDependency>
            Inject<IComponent, IModule, TDependency>(
            this IInterfaceRegistrationContext<IComponent, IModule> component,
            Expression<Func<IModule, TDependency>> dependencySourceExpression)
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            var dependencyContext = contextImpl.Context.InjectSource((LambdaExpression)dependencySourceExpression);

            return new InterfaceDependencyInjectionContext<IComponent, IModule, TDependency>(
                contextImpl, dependencyContext);
        }

        public static IInterfaceRegistrationContext<IComponent, IModule>
            AddInjector<IComponent, IModule>(
            this IInterfaceRegistrationContext<IComponent, IModule> component,
            IInterfaceInjector<IComponent, IModule> injector)
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
        public static IInterfaceRegistrationContext<IComponent, IModule> AddBehaviour<IComponent, IModule, TBehaviour>(
            this IInterfaceRegistrationContext<IComponent, IModule> component,
            TBehaviour behaviour)
            where IModule : Interfaces.IModule
            where TBehaviour : Microsoft.Practices.Unity.InterceptionExtension.IInterceptionBehavior
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.AddBehaviour(behaviour);

            return component;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule> AddCustomAction<IComponent, IModule>(
            this IInterfaceRegistrationContext<IComponent, IModule> component,
            Action<IComponent> customAction)
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.AddCustomAction(customAction);

            return component;
        }

        private static InterfaceRegistrationContext<IComponent, IModule> GetContextImplementation<IComponent, IModule>(IInterfaceRegistrationContext<IComponent, IModule> component)
        {
            return (InterfaceRegistrationContext<IComponent, IModule>)component;
        }

        #endregion
    }
}

