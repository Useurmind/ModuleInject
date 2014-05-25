using Microsoft.Practices.Unity;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject
{
    public static class DependencyInjectionContextExtensions
    {
        public static ComponentRegistrationContext<IComponent, TComponent, IModule> IntoProperty<IComponent, TComponent, IModule, TDependency, TProperty>(
            this DependencyInjectionContext<IComponent, TComponent, IModule, TDependency> dependency,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
        )
            where TComponent : IComponent
            where TProperty : TDependency
        {
            ComponentRegistrationContext<IComponent, TComponent, IModule> component = dependency.ComponentContext;

            string sourceName = dependency.DependencyName;
            string targetName = Property.Get(dependencyTargetExpression);

            component.Container.RegisterType<IComponent, TComponent>(component.ComponentName,
                new InjectionProperty(targetName, new ResolvedParameter<TDependency>(sourceName))
            );

            return component;
        }
    }
}
