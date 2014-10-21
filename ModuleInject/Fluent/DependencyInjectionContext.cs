using Microsoft.Practices.Unity;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Common.Linq;

    internal class DependencyInjectionContext
    {
        public ComponentRegistrationContext ComponentContext { get; private set; }
        public string DependencyName { get; private set; }
        public Type DependencyType { get; private set; }

        public DependencyInjectionContext(ComponentRegistrationContext componentContext, string dependencyName, Type dependencyType)
        {
            ComponentContext = componentContext;
            DependencyName = dependencyName;
            DependencyType = dependencyType;
        }

        public ComponentRegistrationContext IntoProperty(Expression dependencyTargetExpression)
        {
            ComponentRegistrationContext component = this.ComponentContext;
            ComponentRegistrationTypes types = component.Types;

            string sourceName = DependencyName;
            string targetName = Property.Get(dependencyTargetExpression);

            component.Container.RegisterType(types.IComponent, types.TComponent, component.ComponentName,
                new InjectionProperty(targetName, new ResolvedParameter(DependencyType, sourceName))
            );

            return component;
        }
    }
}
