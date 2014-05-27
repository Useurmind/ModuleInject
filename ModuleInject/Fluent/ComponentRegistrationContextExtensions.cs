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
        private static readonly string _initialize1MethodName;
        private static readonly string _initialize2MethodName;
        private static readonly string _initialize3MethodName;

        static ComponentRegistrationContextExtensions()
        {
            _initialize1MethodName = ExtractMethodName<IInitializable<object>>(x => x.Initialize(null));
            _initialize2MethodName = ExtractMethodName<IInitializable<object, object>>(x => x.Initialize(null, null));
            _initialize3MethodName = ExtractMethodName<IInitializable<object, object, object>>(x => x.Initialize(null, null, null));
        }

        public static ValueInjectionContext<IComponent, TComponent, IModule, TDependency> Inject<IComponent, TComponent, IModule, TDependency>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule> component,
            TDependency value)
            where TComponent : IComponent
            where IModule : IInjectionModule
        {
            return new ValueInjectionContext<IComponent, TComponent, IModule, TDependency>(component, value);
        }

        public static DependencyInjectionContext<IComponent, TComponent, IModule, TDependency> Inject<IComponent, TComponent, IModule, TDependency>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule> component,
            Expression<Func<IModule, TDependency>> dependencySourceExpression)
            where TComponent : IComponent
            where IModule : IInjectionModule
        {
            string propertyPath = LinqHelper.GetMemberPath(dependencySourceExpression);

            return new DependencyInjectionContext<IComponent, TComponent, IModule, TDependency>(component, propertyPath);
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule> InitializeWith<IComponent, TComponent, IModule, TDep1>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule> component,
            Expression<Func<IModule, TDep1>> dependency1SourceExpression)
            where TComponent : IComponent, IInitializable<TDep1>
            where IModule : IInjectionModule
        {
            string propertyPath = LinqHelper.GetMemberPath(dependency1SourceExpression);

            component.Container.RegisterType<IComponent, TComponent>(component.ComponentName,
                new InjectionMethod(_initialize1MethodName, new ResolvedParameter<TDep1>(propertyPath)));

            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule> InitializeWith<IComponent, TComponent, IModule, TDep1, TDep2>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule> component,
            Expression<Func<IModule, TDep1>> dependency1SourceExpression,
            Expression<Func<IModule, TDep2>> dependency2SourceExpression)
            where TComponent : IComponent, IInitializable<TDep1, TDep2>
            where IModule : IInjectionModule
        {
            string property1Path = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string property2Path = LinqHelper.GetMemberPath(dependency2SourceExpression);

            component.Container.RegisterType<IComponent, TComponent>(component.ComponentName,
                new InjectionMethod(_initialize2MethodName, 
                    new ResolvedParameter<TDep1>(property1Path),
                    new ResolvedParameter<TDep2>(property2Path)
                    ));

            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule> InitializeWith<IComponent, TComponent, IModule, TDep1, TDep2, TDep3>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule> component,
            Expression<Func<IModule, TDep1>> dependency1SourceExpression,
            Expression<Func<IModule, TDep2>> dependency2SourceExpression,
            Expression<Func<IModule, TDep3>> dependency3SourceExpression)
            where TComponent : IComponent, IInitializable<TDep1, TDep2, TDep3>
            where IModule : IInjectionModule
        {
            string property1Path = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string property2Path = LinqHelper.GetMemberPath(dependency2SourceExpression);
            string property3Path = LinqHelper.GetMemberPath(dependency3SourceExpression);

            component.Container.RegisterType<IComponent, TComponent>(component.ComponentName,
                new InjectionMethod(_initialize3MethodName,
                    new ResolvedParameter<TDep1>(property1Path),
                    new ResolvedParameter<TDep2>(property2Path),
                    new ResolvedParameter<TDep3>(property3Path)
                    ));

            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule> AddInjector<IComponent, TComponent, IModule>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule> component,
            IInjector<IComponent, TComponent, IModule> injector)
            where TComponent : IComponent
            where IModule : IInjectionModule
        {
            injector.InjectInto(component);
            return component;
        }


        private static string ExtractMethodName<TObject>(Expression<Action<TObject>> methodExpression)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)methodExpression.Body;
            return methodCallExpression.Method.Name;
        }
    }
}

