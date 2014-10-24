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
    using ModuleInject.Common.Exceptions;
    using ModuleInject.Container.Interface;
    using ModuleInject.Container.Resolving;

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
        public bool WasConstructorWithArgumentsCalled { get; private set; }

        public string ComponentName { get; private set; }
        public IDependencyContainer Container { get; private set; }
        public InjectionModule Module { get; private set; }

        public ComponentRegistrationContext(string name, InjectionModule module, IDependencyContainer container, ComponentRegistrationTypes types, bool interceptionActive)
        {
            IsInterceptionActive = interceptionActive;
            IsInterceptorAlreadyAdded = false;
            WasConstructorWithArgumentsCalled = false;
            ComponentName = name;
            Module = module;
            Container = container;
            Types = types;
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
            Container.InjectMethod(
                ComponentName,
                Types.IComponent,
                _initialize1MethodName,
                NewResolvedParameter(dependency1SourceExpression));

            return this;
        }

        public ComponentRegistrationContext InitializeWith(
            Expression dependency1SourceExpression,
            Expression dependency2SourceExpression)
        {
            Container.InjectMethod(ComponentName, Types.IComponent,
                _initialize2MethodName,
                    NewResolvedParameter(dependency1SourceExpression),
                    NewResolvedParameter(dependency2SourceExpression)
                    );

            return this;
        }

        public ComponentRegistrationContext InitializeWith(
            Expression dependency1SourceExpression,
            Expression dependency2SourceExpression,
            Expression dependency3SourceExpression)
        {
            Container.InjectMethod(ComponentName, Types.IComponent,
                _initialize3MethodName,
                    NewResolvedParameter(dependency1SourceExpression),
                    NewResolvedParameter(dependency2SourceExpression),
                    NewResolvedParameter(dependency3SourceExpression)
                    );

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructorCallExpression">Expects an expression of the form module => new SomeConstructor(module.SomeComponent, ..).</param>
        /// <returns></returns>
        public ComponentRegistrationContext CallConstructor(LambdaExpression constructorCallExpression)
        {
            IList<MethodCallArgument> arguments;

            arguments = LinqHelper.GetConstructorArguments(constructorCallExpression);

            IResolvedValue[] argumentParams = GetContainerInjectionArguments(arguments);

            Container.InjectConstructor(ComponentName, Types.IComponent,
                argumentParams);

            WasConstructorWithArgumentsCalled = true;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodCallExpression">Expects an expression of the form (component, module) => component.Method(module.SomeComponent, ..).</param>
        /// <returns></returns>
        public ComponentRegistrationContext CallMethod(LambdaExpression methodCallExpression)
        {
            IList<MethodCallArgument> arguments;  // type and path in the module or value for constants etc.
            string methodName;
            LinqHelper.GetMethodNameAndArguments(methodCallExpression, out methodName, out arguments);

            IResolvedValue[] argumentParams = GetContainerInjectionArguments(arguments);

            Container.InjectMethod(ComponentName,Types.IComponent, methodName, argumentParams);

            return this;
        }

        public ComponentRegistrationContext AddBehaviour<TBehaviour>()
            where TBehaviour : ISimpleBehaviour, new()
        {
            // TODO: fix behaviors
            //if (!IsInterceptionActive)
            //{
            //    ExceptionHelper.ThrowTypeException(Types.TModule, Errors.ComponentRegistrationContext_InterceptionNotActivated);
            //}

            //Unity.InterceptionBehavior unityBehaviour = new Unity.InterceptionBehavior<SimpleUnityBehaviour<TBehaviour>>();

            //if (this.IsInterceptorAlreadyAdded)
            //{
            //    this.Container.RegisterType(Types.IComponent, Types.TComponent, this.ComponentName,
            //        unityBehaviour
            //        );
            //}
            //else
            //{
            //    this.Container.RegisterType(Types.IComponent, Types.TComponent, this.ComponentName,
            //        new Unity.Interceptor<Unity.InterfaceInterceptor>(),
            //        unityBehaviour
            //        );
            //    this.IsInterceptorAlreadyAdded = true;
            //}

            return this;
        }

        public ComponentRegistrationContext AlsoRegisterFor(Expression moduleProperty)
        {
            string memberPath;
            Type memberType;
            LinqHelper.GetMemberPathAndType(moduleProperty, out memberPath, out memberType);
            this.Container.Register(memberPath, memberType, cont =>
            {
                return cont.Resolve(this.ComponentName, Types.IComponent);
            });

            return this;
        }

        private IResolvedValue[] GetContainerInjectionArguments(IList<MethodCallArgument> arguments)
        {
            IResolvedValue[] argumentParams = new IResolvedValue[arguments.Count];
            int i = 0;
            foreach (var argumentItem in arguments)
            {
                if (argumentItem.ResolvePath != null)
                {
                    argumentParams[i] = LinqHelper.GetContainerReference(Module, argumentItem.ResolvePath, argumentItem.ArgumentType);
                }
                else
                {
                    argumentParams[i] = new ConstantValue(argumentItem.Value, argumentItem.ArgumentType);
                }
                i++;
            }
            return argumentParams;
        }

        private IResolvedValue NewResolvedParameter(Expression dependencyExpression)
        {
            string memberPath;
            Type memberType;
            LinqHelper.GetMemberPathAndType(dependencyExpression, out memberPath, out memberType);

            return LinqHelper.GetContainerReference(Module, memberPath, memberType);
        }

        private static string ExtractMethodName<TObject>(Expression<Action<TObject>> methodExpression)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)methodExpression.Body;
            return methodCallExpression.Method.Name;
        }

    }
}
