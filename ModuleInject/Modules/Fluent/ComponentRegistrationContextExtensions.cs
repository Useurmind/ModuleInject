using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public static class ComponentRegistrationContextExtensions
    {

        public static IValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            TDependency value)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("componentContext", componentContext);

            var valueContext = componentContext.Context.Inject(value, typeof(TDependency));
            return new ValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(componentContext, valueContext);
        }

        public static IDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            Expression<Func<TModule, TDependency>> dependencySourceExpression)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("componentContext", componentContext);

            var dependencyContext = componentContext.Context.InjectSource((LambdaExpression)dependencySourceExpression);

            return new DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(
                componentContext, dependencyContext);
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
             Inject<IComponent, TComponent, IModule, TModule>(
             this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
             Expression<Action<TComponent, TModule>> methodCallExpression)
             where TComponent : IComponent
             where TModule : IModule
             where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("componentContext", componentContext);

            componentContext.Context.Inject(methodCallExpression);

            return componentContext;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            IClassInjector<IComponent, TComponent, IModule, TModule> injector)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("injector", injector);

            injector.InjectInto(componentContext);
            return componentContext;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            IInterfaceInjector<IComponent, IModule, TModule> injector)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("componentContext", componentContext);
            CommonFunctions.CheckNullArgument("injector", injector);
            
            var interfaceContext = new InterfaceRegistrationContext<IComponent, IModule, TModule>(componentContext.Context);
            injector.InjectInto(interfaceContext);
            return componentContext;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule, IComponentBase, IModuleBase>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            IInterfaceInjector<IComponentBase, IModuleBase> injector)
            where TComponent : IComponent, IComponentBase
            where TModule : IModule, IModuleBase
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("componentContext", componentContext);
            CommonFunctions.CheckNullArgument("injector", injector);

            var interfaceContext = new InterfaceRegistrationContext<IComponentBase, IModuleBase>(componentContext.Context);
            injector.InjectInto(interfaceContext);
            return componentContext;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddTypeInjection<IComponent, TComponent, IModule, TModule>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            Action<IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> injectInto)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            ClassInjector<IComponent, TComponent, IModule, TModule> injector =
                new ClassInjector<IComponent, TComponent, IModule, TModule>(injectInto);

            componentContext.AddInjector(injector);
            return componentContext;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInterfaceInjection<IComponent, TComponent, IModule, TModule>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            Action<IInterfaceRegistrationContext<IComponent, IModule, TModule>> injectInto)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            InterfaceInjector<IComponent, IModule, TModule> injector =
                new InterfaceInjector<IComponent, IModule, TModule>(injectInto);

            componentContext.AddInjector(injector);
            return componentContext;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInterfaceInjection<IComponent, TComponent, IModule, TModule, IComponentBase, IModuleBase>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            Action<IInterfaceRegistrationContext<IComponentBase, IModuleBase>> injectInto)
            where TComponent : IComponent, IComponentBase
            where TModule : IModule, IModuleBase
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            InterfaceInjector<IComponentBase, IModuleBase> injector =
                new InterfaceInjector<IComponentBase, IModuleBase>(injectInto);

            componentContext.AddInjector(injector);
            return componentContext;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AlsoRegisterFor<IComponent, TComponent, IModule, TModule, IComponent2>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            Expression<Func<TModule, IComponent2>> secondModuleProperty)
            where TComponent : IComponent
            where IComponent : IComponent2
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("componentContext", componentContext);

            componentContext.Context.AlsoRegisterFor(secondModuleProperty);

            return componentContext;
        }

        /// <summary>
        /// Add a behavior of the given type.
        /// </summary>
        /// <typeparam name="TBehaviour">The type of the behavior to add.</typeparam>
        /// <returns>The current context of the fluent API.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This API is by design statically typed")]
        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> AddBehaviour<IComponent, TComponent, IModule, TModule, TBehaviour>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            TBehaviour behaviour)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : Interfaces.IModule
            where TBehaviour : Microsoft.Practices.Unity.InterceptionExtension.IInterceptionBehavior
        {
            CommonFunctions.CheckNullArgument("componentContext", componentContext);

            // TODO: fix this hack
            ((RegistrationContext)componentContext.Context).AddBehaviour(behaviour);

            return componentContext;
        }

        /// <summary>
        /// Register a custom action that will run when the component is resolved.
        /// Inside the action only the created instance of the component is available.
        /// The module is not, because it can not be guaranteed that all properties of the 
        /// module are already resolved.
        /// </summary>
        /// <typeparam name="IComponent">The interface of the component.</typeparam>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="IModule">The interface of the module.</typeparam>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="componentContext">The fluent context in which a custom action is registered.</param>
        /// <param name="customAction">The action that should be executed.</param>
        /// <returns></returns>
        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> AddCustomAction<IComponent, TComponent, IModule, TModule>(
            this IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext,
            Action<TComponent> customAction)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("componentContext", componentContext);

            componentContext.Context.AddCustomAction(customAction);

            return componentContext;
        }
    }
}

