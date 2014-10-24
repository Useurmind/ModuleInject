using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Common.Utility;

    public static class InstanceDependencyInjectionContextExtensions
    {
        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            IntoProperty<IComponent, TComponent, IModule, TModule, TProperty>(
            this InstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TProperty> instanceDependencyInject,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
            )
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instanceDependencyInject", instanceDependencyInject);

            string targetPropertyName = LinqHelper.GetMemberPath(dependencyTargetExpression);

            instanceDependencyInject.DependencyInjectionContext.IntoProperty(dependencyTargetExpression);

            return instanceDependencyInject.InstanceContext;
        }
    }
}
