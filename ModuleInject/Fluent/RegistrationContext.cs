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
    using ModuleInject.Container.InstanceCreation;
    using ModuleInject.Container.Dependencies;

    internal class RegistrationTypes : IRegistrationTypes
    {
        public Type IComponent { get; set; }
        public Type TComponent { get; set; }
        public Type IModule { get; set; }
        public Type TModule { get; set; }
    }

    internal class RegistrationContext : IRegistrationContext
    {
        private RegistrationTypes registrationTypes;

        private Dictionary<string, ModifiedDependency> modifiedDependencies; 

        public IRegistrationTypes RegistrationTypes { get { return registrationTypes; } }
        public string RegistrationName { get; private set; }

        public bool WasConstructorCalled { get; private set; }

        public IDependencyContainer Container { get; private set; }
        public InjectionModule Module { get; private set; }

        public RegistrationContext(string name, InjectionModule module, IDependencyContainer container, RegistrationTypes registrationTypes, bool wasConstructorCalled=false)
        {
            this.WasConstructorCalled = wasConstructorCalled;
            Module = module;
            Container = container;
            this.registrationTypes = registrationTypes;
            this.RegistrationName = name;
            modifiedDependencies = new Dictionary<string, ModifiedDependency>();
        }

        private ModifiedDependency GetModifiedDependency(string dependencyPath)
        {
            ModifiedDependency modification = null;

            this.modifiedDependencies.TryGetValue(dependencyPath, out modification);

            return modification;
        }

        public RegistrationContext Construct(object instance)
        {
            if (this.WasConstructorCalled)
            {
                ExceptionHelper.ThrowFormatException(Errors.RegistrationContext_ConstructorAlreadyCalled, this.RegistrationName, this.RegistrationTypes.TModule.Name);
            }

            this.registrationTypes.TComponent = instance.GetType();
            Container.Register(this.RegistrationName, this.RegistrationTypes.IComponent, instance);
            
            this.WasConstructorCalled = true;

            return this;
        }

        public RegistrationContext Construct(Type componentType)
        {
            if (this.WasConstructorCalled)
            {
                ExceptionHelper.ThrowFormatException(Errors.RegistrationContext_ConstructorAlreadyCalled, this.RegistrationName, this.RegistrationTypes.TModule.Name);
            }

            this.registrationTypes.TComponent = componentType;
            Container.Register(this.RegistrationName, this.RegistrationTypes.IComponent, componentType);

            this.WasConstructorCalled = true;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructorCallExpression">Expects an expression of the form module => new SomeConstructor(module.SomeComponent, ..).</param>
        /// <returns></returns>
        public RegistrationContext Construct(LambdaExpression constructorCallExpression)
        {
            if (this.WasConstructorCalled)
            {
                ExceptionHelper.ThrowFormatException(Errors.RegistrationContext_ConstructorAlreadyCalled, this.RegistrationName, this.RegistrationTypes.TModule.Name);
            }

            var dependencyEvaluator = new ParameterMemberAccessEvaluator(constructorCallExpression, 0);
            AddPrerequisites(dependencyEvaluator);

            Delegate compiledConstructorExpression = constructorCallExpression.Compile();

            Func<IDependencyContainer, object> constructorFunc = new Func<IDependencyContainer, object>(cont =>
            {
                return compiledConstructorExpression.DynamicInvoke(this.Module);
            });

            this.Construct(constructorCallExpression.Body.Type);
            Container.SetInstanceCreation(this.RegistrationName, this.RegistrationTypes.IComponent,
                new FactoryInstanceCreation(Container, constructorFunc));

            this.WasConstructorCalled = true;

            return this;
        }

        public ValueInjectionContext Inject(object value, Type valueType)
        {
            return new ValueInjectionContext(this, value, valueType);
        }

        //public RegistrationContext ModifyDependencyBy(Expression dependencySourceExpression, Action<object> modifyAction)
        //{
        //    string memberPath;
        //    Type memberType;

        //    LinqHelper.GetMemberPathAndType(dependencySourceExpression, out memberPath, out memberType);

        //    ModifiedDependency modification = null;
        //    if (!this.modifiedDependencies.TryGetValue(memberPath, out modification))
        //    {
        //        modification = new ModifiedDependency(memberPath, memberType, modifyAction);
        //        this.modifiedDependencies.Add(memberPath, modification);
        //    }
        //    else
        //    {
        //        modification.AddModifyAction(modifyAction);
        //    }

        //    return this;
        //}

        public DependencyInjectionContext Inject(Expression dependencySourceExpression)
        {
            string memberPath;
            Type memberType;

            LinqHelper.GetMemberPathAndType(dependencySourceExpression, out memberPath, out memberType);

            return new DependencyInjectionContext(this, memberPath, memberType);
        }
       
        private void AddPrerequisites(ParameterMemberAccessEvaluator dependencyEvaluator)
        {
            dependencyEvaluator.Evaluate();

            // Only set dependencies in current module as prerequisites, submodules are resolved before this module
            var moduleOnlyDependencies = dependencyEvaluator.MemberPaths.Where(x => x.Depth == 1);
            foreach (var memberPathInformation in moduleOnlyDependencies)
            {
                var containerReference = new ContainerReference(Container, memberPathInformation.Path, memberPathInformation.ReturnType);
                Container.DefinePrerequisite(this.RegistrationName, this.RegistrationTypes.IComponent, containerReference);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodCallExpression">Expects an expression of the form (component, module) => component.Method(module.SomeComponent, ..).</param>
        /// <returns></returns>
        public RegistrationContext Inject(LambdaExpression methodCallExpression)
        {
            ParameterMemberAccessEvaluator dependencyEvaluator = new ParameterMemberAccessEvaluator(methodCallExpression, 1);
            AddPrerequisites(dependencyEvaluator);

            Delegate compiledMethodCallExpression = methodCallExpression.Compile();

            var lambdaInjection = new LambdaDependencyInjection(Container, (c, obj) =>
            {
                var component = Convert.ChangeType(obj, this.RegistrationTypes.TComponent);
                compiledMethodCallExpression.DynamicInvoke(component, this.Module);
            });

            Container.Inject(this.RegistrationName, this.RegistrationTypes.IComponent, lambdaInjection);

            //IList<MethodCallArgument> arguments;  // type and path in the module or value for constants etc.
            //string methodName;
            //LinqHelper.GetMethodNameAndArguments(methodCallExpression, out methodName, out arguments);

            //IResolvedValue[] argumentParams = GetContainerInjectionArguments(arguments);

            //Container.InjectMethod(this.RegistrationName,this.RegistrationTypes.IComponent, methodName, argumentParams);

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
