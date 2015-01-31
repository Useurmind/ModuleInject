using System;
using System.Linq;
using System.Linq.Expressions;

using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public static class InterfaceValueInjectionContextExtensions
    {
        public static IInterfaceRegistrationContext<IComponent, IModule, TModule> 
            IntoProperty<IComponent, IModule, TModule, TDependency, TProperty>(
            this IInterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency> valueContext,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
        )
            where TProperty : TDependency
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("valueContext", valueContext);

            valueContext.Context.IntoProperty(dependencyTargetExpression);

            return valueContext.RegistrationContext;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule>
            IntoProperty<IComponent, IModule, TDependency, TProperty>(
            this IInterfaceValueInjectionContext<IComponent, IModule, TDependency> valueContext,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
        )
            where TProperty : TDependency
        {
            CommonFunctions.CheckNullArgument("valueContext", valueContext);

            valueContext.Context.IntoProperty(dependencyTargetExpression);

            return valueContext.RegistrationContext;
        }
    }
}
