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
    using ModuleInject.Common.Utility;

    public static class InterfaceValueInjectionContextExtensions
    {
        public static InterfaceRegistrationContext<IComponent, IModule, TModule> 
            IntoProperty<IComponent, IModule, TModule, TDependency, TProperty>(
            this InterfaceValueInjectionContext<IComponent, IModule, TModule, TDependency> value,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
        )
            where TProperty : TDependency
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("value", value);

            value.Context.IntoProperty(dependencyTargetExpression);

            return value.ComponentContext;
        }
    }
}
