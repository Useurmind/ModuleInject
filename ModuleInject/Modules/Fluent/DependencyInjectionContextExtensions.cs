using System;
using System.Linq;
using System.Linq.Expressions;

using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public static class DependencyInjectionContextExtensions
    {
        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            IntoProperty<IComponent, TComponent, IModule, TModule, TDependency, TProperty>(
            this IDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> dependency,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
            )
            where TComponent : IComponent
            where TDependency : TProperty
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("dependency", dependency);

            var contextImpl =
                (DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>)dependency;

            contextImpl.Context.IntoProperty(dependencyTargetExpression);

            return contextImpl.ComponentContext;
        }
    }
}
