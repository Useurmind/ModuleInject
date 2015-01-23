using System;
using System.Linq;
using System.Linq.Expressions;

using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public static class ValueInjectionContextExtensions
    {
        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            IntoProperty<IComponent, TComponent, IModule, TModule, TDependency, TProperty>(
            this IValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> value,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
        )
            where TComponent : IComponent
            where TDependency : TProperty
            where TModule : IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("value", value);

            var contextImpl = (ValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>)value;

            contextImpl.Context.IntoProperty(dependencyTargetExpression);

            return contextImpl.ComponentContext;
        }
    }
}
