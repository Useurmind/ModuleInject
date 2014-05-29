﻿using Microsoft.Practices.Unity;
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
        private static readonly string _initialize1MethodName = ExtractMethodName<IInitializable<object>>(x => x.Initialize(null));
        private static readonly string _initialize2MethodName = ExtractMethodName<IInitializable<object, object>>(x => x.Initialize(null, null));
        private static readonly string _initialize3MethodName = ExtractMethodName<IInitializable<object, object, object>>(x => x.Initialize(null, null, null));

        public static ValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            TDependency value)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            return new ValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(component, value);
        }

        public static DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> 
            Inject<IComponent, TComponent, IModule, TModule, TDependency>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<TModule, TDependency>> dependencySourceExpression)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            string memberPath = LinqHelper.GetMemberPath(dependencySourceExpression);

            return new DependencyInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(component, memberPath);
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<IModule, TDependency1>> dependency1SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            string memberPath = LinqHelper.GetMemberPath(dependency1SourceExpression);

            component.Container.RegisterType<IComponent, TComponent>(component.ComponentName,
                new InjectionMethod(_initialize1MethodName, new ResolvedParameter<TDependency1>(memberPath)));

            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<IModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<IModule, TDependency2>> dependency2SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            string memberPath1 = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string memberPath2 = LinqHelper.GetMemberPath(dependency2SourceExpression);

            component.Container.RegisterType<IComponent, TComponent>(component.ComponentName,
                new InjectionMethod(_initialize2MethodName, 
                    new ResolvedParameter<TDependency1>(memberPath1),
                    new ResolvedParameter<TDependency2>(memberPath2)
                    ));

            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            InitializeWith<IComponent, TComponent, IModule, TModule, TDependency1, TDependency2, TDependency3>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            Expression<Func<IModule, TDependency1>> dependency1SourceExpression,
            Expression<Func<IModule, TDependency2>> dependency2SourceExpression,
            Expression<Func<IModule, TDependency3>> dependency3SourceExpression)
            where TComponent : IComponent, IInitializable<TDependency1, TDependency2, TDependency3>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("component", component);

            string memberPath1 = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string memberPath2 = LinqHelper.GetMemberPath(dependency2SourceExpression);
            string memberPath3 = LinqHelper.GetMemberPath(dependency3SourceExpression);

            component.Container.RegisterType<IComponent, TComponent>(component.ComponentName,
                new InjectionMethod(_initialize3MethodName,
                    new ResolvedParameter<TDependency1>(memberPath1),
                    new ResolvedParameter<TDependency2>(memberPath2),
                    new ResolvedParameter<TDependency3>(memberPath3)
                    ));

            return component;
        }

        public static ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddInjector<IComponent, TComponent, IModule, TModule>(
            this ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> component,
            IInjector<IComponent, TComponent, IModule, TModule> injector)
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("injector", injector);

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

