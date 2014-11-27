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

        //public static IInterfaceRegistrationContext<IComponent, IModule, TModule>
        //    ModifyDependencyBy<IComponent, IModule, TModule, TDependency>(
        //    this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
        //    Expression<Func<TModule, TDependency>> dependencySourceExpression,
        //    Action<TDependency> modifyAction)
        //    where TModule : IModule
        //    where IModule : IInjectionModule
        //{
        //    CommonFunctions.CheckNullArgument("component", component);

        //    var contextImpl = GetContextImplementation(component);

        //    contextImpl.Context.ModifyDependencyBy(dependencySourceExpression, obj => modifyAction((TDependency)obj));

        //    return component;
        //}

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

            var dependencyContext = contextImpl.Context.Inject(dependencySourceExpression);

            return new InterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency>(
                contextImpl, dependencyContext);
        }

        public static IInterfaceRegistrationContext<IComponent, IModule, TModule>
            InitializeWith<IComponent, IModule, TModule, TDependency1>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression)
            where IComponent : IInitializable<TDependency1>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.InitializeWith(dependency1SourceExpression);

            return component;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule, TModule>
            InitializeWith<IComponent, IModule, TModule, TDependency1, TDependency2>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<TModule, TDependency2>> dependency2SourceExpression)
            where IComponent : IInitializable<TDependency1, TDependency2>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression);

            return component;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule, TModule> 
            InitializeWith<IComponent, IModule, TModule, TDependency1, TDependency2, TDependency3>(
            this IInterfaceRegistrationContext<IComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<TModule, TDependency2>> dependency2SourceExpression,
            Expression<Func<TModule, TDependency3>> dependency3SourceExpression)
            where IComponent : IInitializable<TDependency1, TDependency2, TDependency3>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression, dependency3SourceExpression);

            return component;
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

        private static InterfaceRegistrationContext<IComponent, IModule, TModule> GetContextImplementation<IComponent, IModule, TModule>(IInterfaceRegistrationContext<IComponent, IModule, TModule> component)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            return (InterfaceRegistrationContext<IComponent, IModule, TModule>)component;
        }

        #endregion
    }

    public static class ComponentAndModuleInterfaceRegistrationContextExtensions{

        #region component and module interface

        //public static IInterfaceRegistrationContext<IComponent, IModule>
        //    ModifyDependencyBy<IComponent, IModule, TDependency>(
        //    this IInterfaceRegistrationContext<IComponent, IModule> component,
        //    Expression<Func<IModule, TDependency>> dependencySourceExpression,
        //    Action<TDependency> modifyAction)
        //{
        //    CommonFunctions.CheckNullArgument("component", component);

        //    var contextImpl = GetContextImplementation(component);

        //    contextImpl.Context.ModifyDependencyBy(dependencySourceExpression, obj => modifyAction((TDependency)obj));

        //    return component;
        //}

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

            var dependencyContext = contextImpl.Context.Inject(dependencySourceExpression);

            return new InterfaceDependencyInjectionContext<IComponent, IModule, TDependency>(
                contextImpl, dependencyContext);
        }

        public static IInterfaceRegistrationContext<IComponent, IModule>
            InitializeWith<IComponent, IModule, TDependency1>(
            this IInterfaceRegistrationContext<IComponent, IModule> component,
            Expression<Func<IModule, TDependency1>> dependency1SourceExpression)
            where IComponent : IInitializable<TDependency1>
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.InitializeWith(dependency1SourceExpression);

            return component;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule>
            InitializeWith<IComponent, IModule, TDependency1, TDependency2>(
            this IInterfaceRegistrationContext<IComponent, IModule> component,
            Expression<Func<IModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<IModule, TDependency2>> dependency2SourceExpression)
            where IComponent : IInitializable<TDependency1, TDependency2>
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression);

            return component;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule>
            InitializeWith<IComponent, IModule, TDependency1, TDependency2, TDependency3>(
            this IInterfaceRegistrationContext<IComponent, IModule> component,
            Expression<Func<IModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<IModule, TDependency2>> dependency2SourceExpression,
            Expression<Func<IModule, TDependency3>> dependency3SourceExpression)
            where IComponent : IInitializable<TDependency1, TDependency2, TDependency3>
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression, dependency3SourceExpression);

            return component;
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
            where IModule : IInjectionModule
            where TBehaviour : Microsoft.Practices.Unity.InterceptionExtension.IInterceptionBehavior
        {
            CommonFunctions.CheckNullArgument("component", component);

            var contextImpl = GetContextImplementation(component);

            contextImpl.Context.AddBehaviour(behaviour);

            return component;
        }

        private static InterfaceRegistrationContext<IComponent, IModule> GetContextImplementation<IComponent, IModule>(IInterfaceRegistrationContext<IComponent, IModule> component)
        {
            return (InterfaceRegistrationContext<IComponent, IModule>)component;
        }

        #endregion
    }
}

