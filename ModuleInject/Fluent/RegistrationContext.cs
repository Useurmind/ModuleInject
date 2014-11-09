using Microsoft.Practices.Unity;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;

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
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    internal class RegistrationTypes : IRegistrationTypes
    {
        public Type IComponent { get; set; }
        public Type TComponent { get; set; }
        public Type IModule { get; set; }
        public Type TModule { get; set; }
    }

    internal class RegistrationContext : IRegistrationContext
    {
        private Dictionary<string, ModifiedDependency> modifiedDependencies; 

        public IRegistrationTypes RegistrationTypes { get; private set; }
        public string RegistrationName { get; private set; }

        public bool IsInterceptorAlreadyAdded { get; private set; }
        public bool IsInterceptionActive { get; private set; }
        public bool WasConstructorWithArgumentsCalled { get; private set; }

        public IDependencyContainer Container { get; private set; }
        public InjectionModule Module { get; private set; }

        public RegistrationContext(string name, InjectionModule module, IDependencyContainer container, RegistrationTypes registrationTypes, bool interceptionActive)
        {
            IsInterceptionActive = interceptionActive;
            IsInterceptorAlreadyAdded = false;
            WasConstructorWithArgumentsCalled = false;
            Module = module;
            Container = container;
            this.RegistrationTypes = registrationTypes;
            this.RegistrationName = name;
            modifiedDependencies = new Dictionary<string, ModifiedDependency>();
        }

        private static readonly string _initialize1MethodName = ExtractMethodName<IInitializable<object>>(x => x.Initialize(null));
        private static readonly string _initialize2MethodName = ExtractMethodName<IInitializable<object, object>>(x => x.Initialize(null, null));
        private static readonly string _initialize3MethodName = ExtractMethodName<IInitializable<object, object, object>>(x => x.Initialize(null, null, null));

        private ModifiedDependency GetModifiedDependency(string dependencyPath)
        {
            ModifiedDependency modification = null;

            this.modifiedDependencies.TryGetValue(dependencyPath, out modification);

            return modification;
        }

        public ValueInjectionContext Inject(object value, Type valueType)
        {
            return new ValueInjectionContext(this, value, valueType);
        }

        public RegistrationContext ModifyDependencyBy(Expression dependencySourceExpression, Action<object> modifyAction)
        {
            string memberPath;
            Type memberType;

            LinqHelper.GetMemberPathAndType(dependencySourceExpression, out memberPath, out memberType);

            ModifiedDependency modification = null;
            if (!this.modifiedDependencies.TryGetValue(memberPath, out modification))
            {
                modification = new ModifiedDependency(memberPath, memberType, modifyAction);
                this.modifiedDependencies.Add(memberPath, modification);
            }
            else
            {
                modification.AddModifyAction(modifyAction);
            }

            return this;
        }

        public DependencyInjectionContext Inject(Expression dependencySourceExpression)
        {
            string memberPath;
            Type memberType;

            LinqHelper.GetMemberPathAndType(dependencySourceExpression, out memberPath, out memberType);

            return new DependencyInjectionContext(this, memberPath, memberType);
        }

        public RegistrationContext InitializeWith(Expression dependency1SourceExpression)
        {
            Container.InjectMethod(
                this.RegistrationName,
                this.RegistrationTypes.IComponent,
                _initialize1MethodName,
                NewResolvedParameter(dependency1SourceExpression));

            return this;
        }

        public RegistrationContext InitializeWith(
            Expression dependency1SourceExpression,
            Expression dependency2SourceExpression)
        {
            Container.InjectMethod(this.RegistrationName, this.RegistrationTypes.IComponent,
                _initialize2MethodName,
                    NewResolvedParameter(dependency1SourceExpression),
                    NewResolvedParameter(dependency2SourceExpression)
                    );

            return this;
        }

        public RegistrationContext InitializeWith(
            Expression dependency1SourceExpression,
            Expression dependency2SourceExpression,
            Expression dependency3SourceExpression)
        {
            Container.InjectMethod(this.RegistrationName, this.RegistrationTypes.IComponent,
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
        public RegistrationContext CallConstructor(LambdaExpression constructorCallExpression)
        {
            IList<MethodCallArgument> arguments;

            arguments = LinqHelper.GetConstructorArguments(constructorCallExpression);

            IResolvedValue[] argumentParams = GetContainerInjectionArguments(arguments);

            Container.InjectConstructor(this.RegistrationName, this.RegistrationTypes.IComponent,
                argumentParams);

            WasConstructorWithArgumentsCalled = true;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodCallExpression">Expects an expression of the form (component, module) => component.Method(module.SomeComponent, ..).</param>
        /// <returns></returns>
        public RegistrationContext CallMethod(LambdaExpression methodCallExpression)
        {
            IList<MethodCallArgument> arguments;  // type and path in the module or value for constants etc.
            string methodName;
            LinqHelper.GetMethodNameAndArguments(methodCallExpression, out methodName, out arguments);

            IResolvedValue[] argumentParams = GetContainerInjectionArguments(arguments);

            Container.InjectMethod(this.RegistrationName,this.RegistrationTypes.IComponent, methodName, argumentParams);

            return this;
        }

        public RegistrationContext AddBehaviour<TBehaviour>(TBehaviour behaviour)
            where TBehaviour : Unity.IInterceptionBehavior
        {
            this.Container.AddBehaviour(this.RegistrationName, this.RegistrationTypes.IComponent, behaviour);

            return this;
        }

        public RegistrationContext AlsoRegisterFor(Expression moduleProperty)
        {
            string memberPath;
            Type memberType;
            LinqHelper.GetMemberPathAndType(moduleProperty, out memberPath, out memberType);
            this.Container.Register(memberPath, memberType, cont =>
            {
                return cont.Resolve(this.RegistrationName, this.RegistrationTypes.IComponent);
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
                    var modifyActions = this.GetModifyActions(argumentItem.ResolvePath);
                    argumentParams[i] = LinqHelper.GetContainerReference(Module, argumentItem.ResolvePath, argumentItem.ArgumentType, modifyActions);
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
            var modifyActions = this.GetModifyActions(memberPath);

            return LinqHelper.GetContainerReference(Module, memberPath, memberType, modifyActions);
        }

        public IList<Action<object>> GetModifyActions(string memberPath)
        {
            ModifiedDependency modifiedDependency = this.GetModifiedDependency(memberPath);
            var modifyActions = modifiedDependency == null ? null : modifiedDependency.ModifyActions;
            return modifyActions;
        }

        private static string ExtractMethodName<TObject>(Expression<Action<TObject>> methodExpression)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)methodExpression.Body;
            return methodCallExpression.Method.Name;
        }

    }
}
