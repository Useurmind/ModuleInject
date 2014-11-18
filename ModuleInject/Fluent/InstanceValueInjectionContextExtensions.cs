using Microsoft.Practices.Unity;

using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Common.Utility;
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public static class InstanceValueInjectionContextExtensions
    {
        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> 
            IntoProperty<IComponent, TComponent, IModule, TModule, TDependency, TProperty>(
            this IInstanceValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> value,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
        )
            where TComponent : IComponent
            where TDependency : TProperty
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("value", value);

            var contextImpl = (InstanceValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>)value;

            contextImpl.Context.IntoProperty(dependencyTargetExpression);

            return contextImpl.InstanceContext;
        }
    }
}
