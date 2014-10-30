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
        public static InstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
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

            return new InstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(instance, dependencyName, dependencyType);
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
           Expression<Func<TModule, TDependency1>> dependency1SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            instance.Context.ComponentRegistrationContext.InitializeWith(dependency1SourceExpression);

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<TModule, TDependency2>> dependency2SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

            instance.Context.ComponentRegistrationContext.InitializeWith(dependency1SourceExpression, dependency2SourceExpression);

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2, TDependency3>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
           Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
           Expression<Func<TModule, TDependency2>> dependency2SourceExpression,
            Expression<Func<TModule, TDependency3>> dependency3SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2, TDependency3>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("instance", instance);

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

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            IInterfaceInjector<IComponent, IModule, TModule> injector)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);
            CommonFunctions.CheckNullArgument("injector", injector);

            var interfaceContext = new InterfaceRegistrationContext<IComponent, IModule, TModule>(component.Context.ComponentRegistrationContext);
            injector.InjectInto(interfaceContext);
            return component;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule, IComponentBase, IModuleBase>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            IInterfaceInjector<IComponentBase, IModuleBase> injector)
            where TComponent : IComponent, IComponentBase
            where TModule : IModule, IModuleBase
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);
            CommonFunctions.CheckNullArgument("injector", injector);

            var interfaceContext = new InterfaceRegistrationContext<IComponentBase, IModuleBase>(component.Context.ComponentRegistrationContext);
            injector.InjectInto(interfaceContext);
            return component;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInterfaceInjection<IComponent, TComponent, IModule, TModule>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Action<InterfaceRegistrationContext<IComponent, IModule, TModule>> injectInto)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            InterfaceInjector<IComponent, IModule, TModule> injector =
                new InterfaceInjector<IComponent, IModule, TModule>(injectInto);

            component.AddInjector(injector);
            return component;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInterfaceInjection<IComponent, TComponent, IModule, TModule, IComponentBase, IModuleBase>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Action<InterfaceRegistrationContext<IComponentBase, IModuleBase>> injectInto)
            where TComponent : IComponent, IComponentBase
            where TModule : IModule, IModuleBase
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            InterfaceInjector<IComponentBase, IModuleBase> injector =
                new InterfaceInjector<IComponentBase, IModuleBase>(injectInto);

            component.AddInjector(injector);
            return component;
        }

    }
}
