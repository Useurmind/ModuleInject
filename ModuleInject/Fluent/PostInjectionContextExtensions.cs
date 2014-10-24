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
    public static class PostInjectionContextExtensions
    {
        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            IntoProperty<IComponent, TComponent, IModule, TModule, TProperty>(
            this PostInjectionContext<IComponent, TComponent, IModule, TModule, TProperty> postInject,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
            )
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            string targetPropertyName = LinqHelper.GetMemberPath(dependencyTargetExpression);

            postInject.DependencyInjectionContext.IntoProperty(dependencyTargetExpression);

            return postInject.InstanceContext;
        }
    }
}
