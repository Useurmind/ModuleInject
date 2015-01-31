using System;
using System.Linq;
using System.Linq.Expressions;

using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public static class InterfaceDependencyInjectionContextExtensions
    {
        public static IInterfaceRegistrationContext<IComponent, IModule, TModule> 
            IntoProperty<IComponent, IModule, TModule, TDependency, TProperty>(
            this IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency> dependencyContext,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
            )
            where TProperty : TDependency
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("dependencyContext", dependencyContext);
            
            dependencyContext.Context.IntoProperty(dependencyTargetExpression);

            return dependencyContext.RegistrationContext;
        }

        public static IInterfaceRegistrationContext<IComponent, IModule>
            IntoProperty<IComponent, IModule, TDependency, TProperty>(
            this IInterfaceDependencyInjectionContext<IComponent, IModule, TDependency> dependencyContext,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
            )
            where TProperty : TDependency
        {
            CommonFunctions.CheckNullArgument("dependency", dependencyContext);

            dependencyContext.Context.IntoProperty(dependencyTargetExpression);

            return dependencyContext.RegistrationContext;
        }
    }
}
