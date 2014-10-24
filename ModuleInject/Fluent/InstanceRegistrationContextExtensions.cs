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

    public static class InstanceRegistrationContextExtensions
    {
        public static PostInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            PostInject<IComponent, TComponent, IModule, TModule, TDependency>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Expression<Func<TModule, TDependency>> dependencySourceExpression
            )
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            string dependencyName =null;
            Type dependencyType = null;
            
            LinqHelper.GetMemberPathAndType(dependencySourceExpression, out dependencyName, out dependencyType);

            return new PostInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(instance, dependencyName, dependencyType);
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            PostInitializeWith<IComponent, TComponent, IModule, TModule, TDependency1>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
           Expression<Func<TModule, TDependency1>> dependency1SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            string memberPath = LinqHelper.GetMemberPath(dependency1SourceExpression);

            instance.Context.ComponentRegistrationContext.InitializeWith(dependency1SourceExpression);

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            PostInitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<TModule, TDependency2>> dependency2SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            string memberPath1 = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string memberPath2 = LinqHelper.GetMemberPath(dependency2SourceExpression);


            instance.Context.ComponentRegistrationContext.InitializeWith(dependency1SourceExpression, dependency2SourceExpression);

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            PostInitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2, TDependency3>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
           Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
           Expression<Func<TModule, TDependency2>> dependency2SourceExpression,
            Expression<Func<TModule, TDependency3>> dependency3SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2, TDependency3>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            string memberPath1 = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string memberPath2 = LinqHelper.GetMemberPath(dependency2SourceExpression);
            string memberPath3 = LinqHelper.GetMemberPath(dependency3SourceExpression);


            instance.Context.ComponentRegistrationContext.InitializeWith(dependency1SourceExpression, dependency2SourceExpression, dependency3SourceExpression);

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            CallMethod<IComponent, TComponent, IModule, TModule>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Expression<Action<TComponent, TModule>> methodCallExpression)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            instance.Context.ComponentRegistrationContext.CallMethod(methodCallExpression);

            return instance;
        }

    }
}
