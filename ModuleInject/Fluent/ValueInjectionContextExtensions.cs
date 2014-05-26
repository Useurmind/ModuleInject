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
    public static class ValueInjectionContextExtensions
    {
        public static ComponentRegistrationContext<IComponent, TComponent, IModule> IntoProperty<IComponent, TComponent, IModule, TDependency, TProperty>(
            this ValueInjectionContext<IComponent, TComponent, IModule, TDependency> value,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
        )
            where TComponent : IComponent
            where TProperty : TDependency
            where IModule : IInjectionModule
        {
            ComponentRegistrationContext<IComponent, TComponent, IModule> component = value.ComponentContext;

            string targetName = Property.Get(dependencyTargetExpression);

            component.Container.RegisterType<IComponent, TComponent>(component.ComponentName,
                new InjectionProperty(targetName, value.Value)
            );

            return component;
        }
    }
}
