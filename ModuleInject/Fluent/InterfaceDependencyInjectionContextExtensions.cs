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
    public static class InterfaceDependencyInjectionContextExtensions
    {
        public static InterfaceRegistrationContext<IComponent, IModule, TModule> 
            IntoProperty<IComponent, IModule, TModule, TDependency, TProperty>(
            this InterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependency> dependency,
            Expression<Func<IComponent, TProperty>> dependencyTargetExpression
            )
            where TProperty : TDependency
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("dependency", dependency);

            dependency.Context.IntoProperty(dependencyTargetExpression);

            return dependency.ComponentContext;
        }
    }
}