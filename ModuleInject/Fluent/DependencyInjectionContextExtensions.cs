using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Fluent
{
    public static class DependencyInjectionContextExtensions
    {
        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            IntoProperty<IComponent, TComponent, IModule, TModule, TDependency, TProperty>(
            this DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> dependency,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
            )
            where TComponent : IComponent
            where TProperty : TDependency
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("dependency", dependency);

            dependency.Context.IntoProperty(dependencyTargetExpression);

            return dependency.ComponentContext;
        }
    }
}
