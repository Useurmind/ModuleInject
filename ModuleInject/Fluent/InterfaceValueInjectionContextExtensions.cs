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

    public static class InterfaceValueInjectionContextExtensions
    {
        public static IInterfaceRegistrationContext<IComponent, IModule, TModule> 
            IntoProperty<IComponent, IModule, TModule, TDependency, TProperty>(
            this IInterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency> value,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
        )
            where TProperty : TDependency
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("value", value);

            var contextImpl = GetContextImplementation(value);

            contextImpl.Context.IntoProperty(dependencyTargetExpression);

            return contextImpl.ComponentContext;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule>
            IntoProperty<IComponent, IModule, TDependency, TProperty>(
            this IInterfaceValueInjectionContext<IComponent, IModule, TDependency> value,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
        )
            where TProperty : TDependency
        {
            CommonFunctions.CheckNullArgument("value", value);

            var contextImpl = GetContextImplementation(value);

            contextImpl.Context.IntoProperty(dependencyTargetExpression);

            return contextImpl.ComponentContext;
        }

        private static InterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency> GetContextImplementation<IComponent, IModule, TModule, TDependency>(IInterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency> value)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            return (InterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency>)value;
        }

        private static InterfaceValueInjectionContext<IComponent, IModule, TDependency> GetContextImplementation<IComponent, IModule, TDependency>(IInterfaceValueInjectionContext<IComponent, IModule, TDependency> value)
        {
            return (InterfaceValueInjectionContext<IComponent, IModule, TDependency>)value;
        }
    }
}
