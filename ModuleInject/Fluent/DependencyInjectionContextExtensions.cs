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

    public static class DependencyInjectionContextExtensions
    {
        public static IDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
           ModifiedBy<IComponent, TComponent, IModule, TModule, TDependency>(
           this IDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> dependency,
           Action<TDependency> modificationAction
           )
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("dependency", dependency);

            var contextImpl =
                (DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>)dependency;

            contextImpl.Context.ModifiedBy(obj => modificationAction((TDependency)obj));

            return dependency;
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            IntoProperty<IComponent, TComponent, IModule, TModule, TDependency, TProperty>(
            this IDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> dependency,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
            )
            where TComponent : IComponent
            where TDependency : TProperty
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("dependency", dependency);

            var contextImpl =
                (DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>)dependency;

            contextImpl.Context.IntoProperty(dependencyTargetExpression);

            return contextImpl.ComponentContext;
        }
    }
}
