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
        public string DependencyPath { get; private set; }
        public Type DependencyType { get; private set; }

        public DependencyInjectionContext(ComponentRegistrationContext componentContext, string dependencyPath, Type dependencyType)
        {
            ComponentContext = componentContext;
            this.DependencyPath = dependencyPath;
            DependencyType = dependencyType;
        }

        public ComponentRegistrationContext IntoProperty(Expression dependencyTargetExpression)
        {
            ComponentRegistrationContext component = this.ComponentContext;
            ComponentRegistrationTypes types = component.Types;

            string sourceName = this.DependencyPath;
            string targetName = Property.Get(dependencyTargetExpression);
            var modifyActions = this.ComponentContext.GetModifyActions(this.DependencyPath);

            var containerReference = LinqHelper.GetContainerReference(component.Module, sourceName, DependencyType, modifyActions);

            component.Container.InjectProperty(component.ComponentName,types.IComponent, targetName, containerReference);

            return component;
        }
    }
}
