using Microsoft.Practices.Unity;

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
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public static class InstanceDependencyInjectionContextExtensions
    {
        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            IntoProperty<IComponent, TComponent, IModule, TModule, TDependency, TProperty>(
            this IInstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> instanceDependencyInject,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
            )
            where TDependency : TProperty
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instanceDependencyInject", instanceDependencyInject);

            var contextImpl =
                (InstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>)
                instanceDependencyInject;

            contextImpl.DependencyInjectionContext.IntoProperty(dependencyTargetExpression);

            return contextImpl.InstanceContext;
        }
    }
}
