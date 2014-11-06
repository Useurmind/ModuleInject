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
        public static IInstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TProperty>
            ModifiedBy<IComponent, TComponent, IModule, TModule, TProperty>(
            this IInstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TProperty> instanceDependencyInject,
            Action<TProperty> modifyAction
            )
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instanceDependencyInject", instanceDependencyInject);

            var contextImpl =
                (InstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TProperty>)
                instanceDependencyInject;

            contextImpl.DependencyInjectionContext.ModifiedBy(obj => modifyAction((TProperty)obj));

            return instanceDependencyInject;
        }

        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            IntoProperty<IComponent, TComponent, IModule, TModule, TProperty>(
            this IInstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TProperty> instanceDependencyInject,
            Expression<Func<TComponent, TProperty>> dependencyTargetExpression
            )
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instanceDependencyInject", instanceDependencyInject);

            var contextImpl =
                (InstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TProperty>)
                instanceDependencyInject;

            contextImpl.DependencyInjectionContext.IntoProperty(dependencyTargetExpression);

            return contextImpl.InstanceContext;
        }
    }
}
