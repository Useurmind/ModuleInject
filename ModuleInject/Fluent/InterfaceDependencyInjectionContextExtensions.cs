using Microsoft.Practices.Unity;

using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Common.Utility;
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public static class InterfaceDependencyInjectionContextExtensions
    {
        public static IInterfaceRegistrationContext<IComponent, IModule, TModule> 
            IntoProperty<IComponent, IModule, TModule, TDependency, TProperty>(
            this IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency> dependency,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
            )
            where TProperty : TDependency
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("dependency", dependency);

            var contextImpl = GetContextImplementation(dependency);

            contextImpl.Context.IntoProperty(dependencyTargetExpression);

            return contextImpl.ComponentContext;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule>
            IntoProperty<IComponent, IModule, TDependency, TProperty>(
            this IInterfaceDependencyInjectionContext<IComponent, IModule, TDependency> dependency,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
            )
            where TProperty : TDependency
        {
            CommonFunctions.CheckNullArgument("dependency", dependency);

            var contextImpl = GetContextImplementation(dependency);

            contextImpl.Context.IntoProperty(dependencyTargetExpression);

            return contextImpl.ComponentContext;
        }

        private static InterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency> GetContextImplementation<IComponent, IModule, TModule, TDependency>(IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency> dependency)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            return (InterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency>)dependency;
        }

        private static InterfaceDependencyInjectionContext<IComponent, IModule, TDependency> GetContextImplementation<IComponent, IModule, TDependency>(IInterfaceDependencyInjectionContext<IComponent, IModule, TDependency> dependency)
        {
            return (InterfaceDependencyInjectionContext<IComponent, IModule, TDependency>)dependency;
        }
    }
}
