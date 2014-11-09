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
    using ModuleInject.Interfaces.Fluent;

    internal class DependencyInjectionContext
    {
        public RegistrationContext registrationContext { get; private set; }
        public string DependencyPath { get; private set; }
        public Type DependencyType { get; private set; }

        public DependencyInjectionContext(RegistrationContext registrationContext, string dependencyPath, Type dependencyType)
        {
            this.registrationContext = registrationContext;
            this.DependencyPath = dependencyPath;
            DependencyType = dependencyType;
        }

        public RegistrationContext IntoProperty(Expression dependencyTargetExpression)
        {
            RegistrationContext component = this.registrationContext;
            IRegistrationTypes types = component.RegistrationTypes;

            string sourceName = this.DependencyPath;
            string targetName = Property.Get(dependencyTargetExpression);
            var modifyActions = this.registrationContext.GetModifyActions(this.DependencyPath);

            var containerReference = LinqHelper.GetContainerReference(component.Module, sourceName, DependencyType, modifyActions);

            component.Container.InjectProperty(component.RegistrationName,types.IComponent, targetName, containerReference);

            return component;
        }
    }
}
