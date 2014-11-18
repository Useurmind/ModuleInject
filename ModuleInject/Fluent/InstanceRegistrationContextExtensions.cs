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

    public static class InstanceRegistrationContextExtensions
    {
        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            ModifyDependencyBy<IComponent, TComponent, IModule, TModule, TDependency>(
            this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Expression<Func<TModule, TDependency>> dependencySourceExpression,
            Action<TDependency> modifyAction)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            var contextImpl = GetContextImplementation(instance);

            contextImpl.Context.ModifyDependencyBy(dependencySourceExpression, obj => modifyAction((TDependency)obj));

            return instance;
        }

        public static IInstanceValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            TDependency value)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            var contextImpl = GetContextImplementation(instance);

            var valueContext = contextImpl.Context.Inject(value, typeof(TDependency));
            return new InstanceValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(contextImpl, valueContext);
        }

        public static IInstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Expression<Func<TModule, TDependency>> dependencySourceExpression
            )
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            string dependencyName = null;
            Type dependencyType = null;

            LinqHelper.GetMemberPathAndType(dependencySourceExpression, out dependencyName, out dependencyType);

            var contextImpl = GetContextImplementation(instance);

            return new InstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(contextImpl, dependencyName, dependencyType);
        }

        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1>(
           this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
           Expression<Func<TModule, TDependency1>> dependency1SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            var contextImpl = GetContextImplementation(instance);

            contextImpl.Context.InitializeWith(dependency1SourceExpression);

            return instance;
        }

        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2>(
            this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<TModule, TDependency2>> dependency2SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            var contextImpl = GetContextImplementation(instance);

            contextImpl.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression);

            return instance;
        }

        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2, TDependency3>(
           this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
           Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
           Expression<Func<TModule, TDependency2>> dependency2SourceExpression,
            Expression<Func<TModule, TDependency3>> dependency3SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2, TDependency3>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            var contextImpl = GetContextImplementation(instance);

            contextImpl.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression, dependency3SourceExpression);

            return instance;
        }

        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> CallMethod<IComponent, TComponent, IModule, TModule>(
            this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Expression<Action<TComponent, TModule>> methodCallExpression)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            var contextImpl = GetContextImplementation(instance);

            contextImpl.Context.CallMethod(methodCallExpression);

            return instance;
        }

        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> AddInjector<IComponent, TComponent, IModule, TModule>(
            this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            IInterfaceInjector<IComponent, IModule, TModule> injector)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);
            CommonFunctions.CheckNullArgument("injector", injector);

            var contextImpl = GetContextImplementation(instance);

            var interfaceContext = new InterfaceRegistrationContext<IComponent, IModule, TModule>(contextImpl.Context);
            injector.InjectInto(interfaceContext);
            return instance;
        }

        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> AddInjector<IComponent, TComponent, IModule, TModule, IComponentBase, IModuleBase>(
            this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            IInterfaceInjector<IComponentBase, IModuleBase> injector)
            where TComponent : IComponent, IComponentBase
            where TModule : IModule, IModuleBase
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);
            CommonFunctions.CheckNullArgument("injector", injector);

            var contextImpl = GetContextImplementation(instance);

            var interfaceContext = new InterfaceRegistrationContext<IComponentBase, IModuleBase>(contextImpl.Context);
            injector.InjectInto(interfaceContext);
            return instance;
        }

        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> AddInterfaceInjection<IComponent, TComponent, IModule, TModule>(
            this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Action<IInterfaceRegistrationContext<IComponent, IModule, TModule>> injectInto)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            InterfaceInjector<IComponent, IModule, TModule> injector =
                new InterfaceInjector<IComponent, IModule, TModule>(injectInto);

            instance.AddInjector(injector);
            return instance;
        }

        public static IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> AddInterfaceInjection<IComponent, TComponent, IModule, TModule, IComponentBase, IModuleBase>(
            this IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Action<IInterfaceRegistrationContext<IComponentBase, IModuleBase>> injectInto)
            where TComponent : IComponent, IComponentBase
            where TModule : IModule, IModuleBase
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            InterfaceInjector<IComponentBase, IModuleBase> injector =
                new InterfaceInjector<IComponentBase, IModuleBase>(injectInto);

            instance.AddInjector(injector);
            return instance;
        }

        private static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> GetContextImplementation<IComponent, TComponent, IModule, TModule>(IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            return (InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>)instance;
        }
    }
}
