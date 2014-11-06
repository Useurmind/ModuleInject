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
    using ModuleInject.Container.Resolving;

    internal class DependencyInjectionContext
    {
        public ComponentRegistrationContext ComponentContext { get; private set; }
        public string DependencyName { get; private set; }
        public Type DependencyType { get; private set; }
        public IList<Action<object>> ModifyActions { get; private set; } 

        public DependencyInjectionContext(ComponentRegistrationContext componentContext, string dependencyName, Type dependencyType)
        {
            ComponentContext = componentContext;
            DependencyName = dependencyName;
            DependencyType = dependencyType;
            ModifyActions = new List<Action<object>>();
        }

        public DependencyInjectionContext ModifiedBy(Action<object> modifyAction)
        {
            ModifyActions.Add(modifyAction);

            return this;
        }

        public ComponentRegistrationContext IntoProperty(Expression dependencyTargetExpression)
        {
            ComponentRegistrationContext component = this.ComponentContext;
            ComponentRegistrationTypes types = component.Types;

            string sourceName = DependencyName;
            string targetName = Property.Get(dependencyTargetExpression);

            var containerReference = LinqHelper.GetContainerReference(component.Module, sourceName, DependencyType, ModifyActions);

            component.Container.InjectProperty(component.ComponentName,types.IComponent, targetName, containerReference);

            return component;
        }
    }
}
