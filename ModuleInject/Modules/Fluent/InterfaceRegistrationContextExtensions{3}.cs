using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public static class ComponentInterfaceRegistrationContextExtensions
    {
        #region component interface
        
        public static IInterfaceRegistrationContext<IComponent, IModule, TModule>
             Inject<IComponent, IModule, TModule>(
             this IInterfaceRegistrationContext<IComponent, IModule, TModule> interfaceContext,
             Expression<Action<IComponent, TModule>> methodCallExpression)
             where TModule : IModule
             where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("interfaceContext", interfaceContext);

            interfaceContext.Context.Inject(methodCallExpression);

            return interfaceContext;
        }

        public static IInterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency>
            Inject<IComponent, IModule, TModule, TDependency>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> interfaceContext,
            TDependency value)
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("interfaceContext", interfaceContext);

            var valueContext = interfaceContext.Context.Inject(value, typeof(TDependency));
            return new InterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency>(interfaceContext, valueContext);
        }

        public static IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency>
            Inject<IComponent, IModule, TModule, TDependency>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> interfaceContext,
            Expression<Func<TModule, TDependency>> dependencySourceExpression)
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("interfaceContext", interfaceContext);
            
            var dependencyContext = interfaceContext.Context.InjectSource((LambdaExpression)dependencySourceExpression);

            return new InterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency>(
                interfaceContext, dependencyContext);
        }

        public static IInterfaceRegistrationContext<IComponent, IModule, TModule>
            AddInjector<IComponent, IModule, TModule>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> interfaceContext,
            IInterfaceInjector<IComponent, IModule, TModule> injector)
            where TModule : IModule
            where IModule : Interfaces.IModule
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
        public static IInterfaceRegistrationContext<IComponent, IModule, TModule> AddBehaviour<IComponent, IModule, TModule, TBehaviour>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> interfaceContext,
            TBehaviour behaviour)
            where TModule : IModule
            where IModule : Interfaces.IModule
            where TBehaviour : Microsoft.Practices.Unity.InterceptionExtension.IInterceptionBehavior
        {
            CommonFunctions.CheckNullArgument("interfaceContext", interfaceContext);

            // TODO: fix this hack
            ((RegistrationContext)interfaceContext.Context).AddBehaviourInternal(behaviour);

            return interfaceContext;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule, TModule> AddCustomAction<IComponent, IModule, TModule>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> interfaceContext,
            Action<IComponent> customAction)
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("interfaceContext", interfaceContext);

            interfaceContext.Context.AddCustomAction(customAction);

            return interfaceContext;
        }

        #endregion
    }
}

