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
    public static class ComponentRegistrationContextExtensions
    {
        public static ValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            TDependency value)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var valueContext = component.Context.Inject(value, typeof(TDependency));
            return new ValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(component, valueContext);
        }

        public static DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> 
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency>> dependencySourceExpression)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            var dependencyContext = component.Context.Inject(dependencySourceExpression);

            return new DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(
                component, dependencyContext);
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            component.Context.InitializeWith(dependency1SourceExpression);

            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<TModule, TDependency2>> dependency2SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            component.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression);

            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2, TDependency3>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<TModule, TDependency2>> dependency2SourceExpression,
            Expression<Func<TModule, TDependency3>> dependency3SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2, TDependency3>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            component.Context.InitializeWith(dependency1SourceExpression, dependency2SourceExpression, dependency3SourceExpression);

            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            IClassInjector<IComponent, TComponent, IModule, TModule> injector)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injector", injector);

            injector.InjectInto(component);
            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            IInterfaceInjector<IComponent, IModule, TModule> injector)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injector", injector);

            var interfaceContext = new InterfaceRegistrationContext<IComponent, IModule, TModule>(component.Context);
            injector.InjectInto(interfaceContext);
            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddTypeInjection<IComponent, TComponent, IModule, TModule>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Action<ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> injectInto)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injectInto", injectInto);

            ClassInjector<IComponent, TComponent, IModule, TModule> injector =
                new ClassInjector<IComponent, TComponent, IModule, TModule>(injectInto);

            component.AddInjector(injector);
            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInterfaceInjection<IComponent, TComponent, IModule, TModule>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
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
    }
}

