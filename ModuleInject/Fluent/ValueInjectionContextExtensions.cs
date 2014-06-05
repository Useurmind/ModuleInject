﻿using Microsoft.Practices.Unity;
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
        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            IntoProperty<IComponent, TComponent, IModule, TModule, TDependency, TProperty>(
            this ValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> value,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
        )
            where TComponent : IComponent
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
