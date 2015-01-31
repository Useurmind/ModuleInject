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
             this IInterfaceRegistrationContext<IComponent, IModule> interfaceContext,
             Expression<Action<IComponent>> methodCallExpression)
             where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("interfaceContext", interfaceContext);

            interfaceContext.Context.Inject(methodCallExpression);

            return interfaceContext;
        }

        public static IInterfaceValueInjectionContext<IComponent, IModule, TDependency>
            Inject<IComponent, IModule, TDependency>(
            this IInterfaceRegistrationContext<IComponent, IModule> interfaceContext,
            TDependency value)
        {
            CommonFunctions.CheckNullArgument("interfaceContext", interfaceContext);

            var valueContext = interfaceContext.Context.Inject(value, typeof(TDependency));
            return new InterfaceValueInjectionContext<IComponent, IModule, TDependency>(interfaceContext, valueContext);
        }

        public static IInterfaceDependencyInjectionContext<IComponent, IModule, TDependency>
            Inject<IComponent, IModule, TDependency>(
            this IInterfaceRegistrationContext<IComponent, IModule> interfaceContext,
            Expression<Func<IModule, TDependency>> dependencySourceExpression)
        {
            CommonFunctions.CheckNullArgument("interfaceContext", interfaceContext);

            var dependencyContext = interfaceContext.Context.InjectSource((LambdaExpression)dependencySourceExpression);

            return new InterfaceDependencyInjectionContext<IComponent, IModule, TDependency>(
                interfaceContext, dependencyContext);
        }

        public static IInterfaceRegistrationContext<IComponent, IModule>
            AddInjector<IComponent, IModule>(
            this IInterfaceRegistrationContext<IComponent, IModule> interfaceContext,
            IInterfaceInjector<IComponent, IModule> injector)
        {
            CommonFunctions.CheckNullArgument("injector", injector);

            injector.InjectInto(interfaceContext);
            return interfaceContext;
        }

        /// <summary>
        /// Add a behavior of the given type.
        /// </summary>
        /// <typeparam name="TBehaviour">The type of the behavior to add.</typeparam>
        /// <returns>The current context of the fluent API.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This API is by design statically typed")]
        public static IInterfaceRegistrationContext<IComponent, IModule> AddBehaviour<IComponent, IModule, TBehaviour>(
            this IInterfaceRegistrationContext<IComponent, IModule> interfaceContext,
            TBehaviour behaviour)
            where IModule : Interfaces.IModule
            where TBehaviour : Microsoft.Practices.Unity.InterceptionExtension.IInterceptionBehavior
        {
            CommonFunctions.CheckNullArgument("component", interfaceContext);

            // TODO: fix this hack
            ((RegistrationContext)interfaceContext.Context).AddBehaviour(behaviour);

            return interfaceContext;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule> AddCustomAction<IComponent, IModule>(
            this IInterfaceRegistrationContext<IComponent, IModule> interfaceContext,
            Action<IComponent> customAction)
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("interfaceContext", interfaceContext);

            interfaceContext.Context.AddCustomAction(customAction);

            return interfaceContext;
        }

        #endregion
    }
}

