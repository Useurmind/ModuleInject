using Microsoft.Practices.Unity;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;
using ModuleInject.Interception;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Utility;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ModuleInject.Fluent
{
    internal class ComponentRegistrationTypes
    {
        public Type IComponent { get; set; }
        public Type TComponent { get; set; }          
        public Type IModule { get; set; }
        public Type TModule { get; set; }
    }

    internal class ComponentRegistrationContext
    {
        public ComponentRegistrationTypes Types { get; private set; }
        public bool IsInterceptorAlreadyAdded { get; private set; }
        public bool IsInterceptionActive { get; private set; }

        public string ComponentName { get; private set; }
        public IUnityContainer Container { get; private set; }

        public ComponentRegistrationContext(string name, IUnityContainer container, ComponentRegistrationTypes types, bool interceptionActive)
        {
            IsInterceptionActive = interceptionActive;
            IsInterceptorAlreadyAdded = false;
            ComponentName = name;
            Container = container;
        }

        private static readonly string _initialize1MethodName = ExtractMethodName<IInitializable<object>>(x => x.Initialize(null));
        private static readonly string _initialize2MethodName = ExtractMethodName<IInitializable<object, object>>(x => x.Initialize(null, null));
        private static readonly string _initialize3MethodName = ExtractMethodName<IInitializable<object, object, object>>(x => x.Initialize(null, null, null));

        public ValueInjectionContext Inject(object value, Type valueType)
        {
            return new ValueInjectionContext(this, value, valueType);
        }

        public DependencyInjectionContext Inject(Expression dependencySourceExpression)
        {
            string memberPath;
            Type memberType;
            
            LinqHelper.GetMemberPathAndType(dependencySourceExpression, out memberPath, out memberType);

            return new DependencyInjectionContext(this, memberPath, memberType);
        }

        public ComponentRegistrationContext InitializeWith(Expression dependency1SourceExpression)
        {
            Container.RegisterType(Types.IComponent, Types.TComponent, ComponentName,
                new InjectionMethod(_initialize1MethodName, NewResolvedParameter(dependency1SourceExpression)));

            return this;
        }

        public ComponentRegistrationContext InitializeWith(
            Expression dependency1SourceExpression,
            Expression dependency2SourceExpression)
        {
            Container.RegisterType(Types.IComponent, Types.TComponent, ComponentName,
                new InjectionMethod(_initialize2MethodName,
                    NewResolvedParameter(dependency1SourceExpression),
                    NewResolvedParameter(dependency2SourceExpression)
                    ));

            return this;
        }

        public ComponentRegistrationContext InitializeWith(
            Expression dependency1SourceExpression,
            Expression dependency2SourceExpression,
            Expression dependency3SourceExpression)
        {
            Container.RegisterType(Types.IComponent, Types.TComponent, ComponentName,
                new InjectionMethod(_initialize3MethodName,
                    NewResolvedParameter(dependency1SourceExpression),
                    NewResolvedParameter(dependency2SourceExpression),
                    NewResolvedParameter(dependency3SourceExpression)
                    ));

            return this;
        }

        public ComponentRegistrationContext AddBehaviour<TBehaviour>()
            where TBehaviour : ISimpleBehaviour, new()
        {
            if (!IsInterceptionActive)
            {
                CommonFunctions.ThrowTypeException(Types.TModule, Errors.ComponentRegistrationContext_InterceptionNotActivated);
            }

            Unity.InterceptionBehavior unityBehaviour = new Unity.InterceptionBehavior<SimpleUnityBehaviour<TBehaviour>>();

            if (this.IsInterceptorAlreadyAdded)
            {
                this.Container.RegisterType(Types.IComponent, Types.TComponent, this.ComponentName,
                    unityBehaviour
                    );
            }
            else
            {
                this.Container.RegisterType(Types.IComponent, Types.TComponent, this.ComponentName,
                    new Unity.Interceptor<Unity.InterfaceInterceptor>(),
                    unityBehaviour
                    );
                this.IsInterceptorAlreadyAdded = true;
            }

            return this;
        }


        private static ResolvedParameter NewResolvedParameter(Expression dependencyExpression)
        {
            string memberPath;
            Type memberType;
            LinqHelper.GetMemberPathAndType(dependencyExpression, out memberPath, out memberType);

            return new ResolvedParameter(memberType, memberPath);
        }

        private static string ExtractMethodName<TObject>(Expression<Action<TObject>> methodExpression)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)methodExpression.Body;
            return methodCallExpression.Method.Name;
        }
    }
}
