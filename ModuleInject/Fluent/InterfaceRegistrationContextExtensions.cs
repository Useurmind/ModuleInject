using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Common.Utility;

    public static class InterfaceRegistrationContextExtensions
    {
        public static InterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency>
            Inject<IComponent, IModule, TModule, TDependency>(
            this InterfaceRegistrationContext<IComponent, IModule, TModule> component,
            TDependency value)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var valueContext = component.Context.Inject(value, typeof(TDependency));
            return new InterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency>(component, valueContext);
        }

        public static InterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency> 
            Inject<IComponent, IModule, TModule, TDependency>(
            this InterfaceRegistrationContext<IComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency>> dependencySourceExpression)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var dependencyContext = component.Context.Inject(dependencySourceExpression);

            return new InterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency>(
                component, dependencyContext);
        }

        public static InterfaceRegistrationContext<IComponent, IModule, TModule>
            InitializeWith<IComponent, IModule, TModule, TDependency1>(
            this InterfaceRegistrationContext<IComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression)
            where IComponent : IInitializable<TDependency1>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            component.Context.InitializeWith(dependency1SourceExpression);

            return component;
        }

        public static InterfaceRegistrationContext<IComponent, IModule, TModule>
            InitializeWith<IComponent, IModule, TModule, TDependency1, TDependency2>(
            this InterfaceRegistrationContext<IComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<TModule, TDependency2>> dependency2SourceExpression)
            where IComponent : IInitializable<TDependency1, TDependency2>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            component.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression);

            return component;
        }

        public static InterfaceRegistrationContext<IComponent, IModule, TModule> 
            InitializeWith<IComponent, IModule, TModule, TDependency1, TDependency2, TDependency3>(
            this InterfaceRegistrationContext<IComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<TModule, TDependency2>> dependency2SourceExpression,
            Expression<Func<TModule, TDependency3>> dependency3SourceExpression)
            where IComponent : IInitializable<TDependency1, TDependency2, TDependency3>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            component.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression, dependency3SourceExpression);

            return component;
        }

        public static InterfaceRegistrationContext<IComponent, IModule, TModule>
            AddInjector<IComponent, IModule, TModule>(
            this InterfaceRegistrationContext<IComponent, IModule, TModule> component,
            IInterfaceInjector<IComponent, IModule, TModule> injector)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injector", injector);

            injector.InjectInto(component);
            return component;
        }
    }
}

