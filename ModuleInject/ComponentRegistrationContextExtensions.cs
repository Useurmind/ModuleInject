using Microsoft.Practices.Unity;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject
{
    public static class ComponentRegistrationContextExtensions
    {
        public static DependencyInjectionContext<IComponent, TComponent, IModule, TDependency> Inject<IComponent, TComponent, IModule, TDependency>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule> component,
            Expression<Func<IModule, TDependency>> dependencySourceExpression)
            where TComponent : IComponent
        {
            string propertyPath = LinqHelper.GetMemberPath(dependencySourceExpression);

            return new DependencyInjectionContext<IComponent, TComponent, IModule, TDependency>(component, propertyPath);
        }
    }
}

