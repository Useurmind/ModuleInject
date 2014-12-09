﻿using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using ModuleInject.Fluent;
using ModuleInject.Module;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject
{
    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Linq;
    using ModuleInject.Common.Utility;
    using ModuleInject.Container;
    using ModuleInject.Container.Interface;
    using ModuleInject.Container.Lifetime;
    using ModuleInject.Decoration;
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;
    using ModuleInject.Registry;
    
    /// <summary>
    /// Base class for all modules that you want to build.
    /// </summary>
    /// <typeparam name="IModule">The interface of the module.</typeparam>
    /// <typeparam name="TModule">The type of the module.</typeparam>
    public abstract class InjectionModule<IModule, TModule> : InjectionModule
        where TModule : InjectionModule<IModule, TModule>, IModule
        where IModule : IInjectionModule
    {
        private IDependencyContainer _container;

        private IRegistry _registry;

        private bool _isResolved;
        private bool isResolving;

        internal override Type ModuleInterface
        {
            get
            {
                return typeof(IModule);
            }
        }

        internal override Type ModuleType
        {
            get
            {
                return typeof(TModule);
            }
        }

        internal override IDependencyContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        /// Is the module already resolved.
        /// </summary>
        public override bool IsResolved
        {
            get
            {
                return _isResolved;
            }
        }

        public IRegistry Registry
        {
            get
            {
                if (_registry as EmptyRegistry != null)
                {
                    return null;
                }

                return _registry;
            }
            set
            {
                if (value == null)
                {
                    _registry = new Registry.EmptyRegistry();
                }
                _registry = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectionModule{IModule, TModule}"/> class.
        /// </summary>
        protected InjectionModule()
        {
            isResolving = false;
            _isResolved = false;
            _container = new DependencyContainer();
            _registry = new Registry.EmptyRegistry();

            if (!typeof(IModule).IsInterface)
            {
                ExceptionHelper.ThrowTypeException<IModule>(Errors.InjectionModule_ModulesMustHaveAnInterface);
            }
        }

        /// <summary>
        /// Called before the <see cref="Resolve"/> method is executed.
        /// </summary>
        protected virtual void BeforeResolving() { }

        /// <summary>
        /// Called before the post resolve assembly of objects is executed.
        /// </summary>
        protected virtual void BeforePostResolveAssembly() { }

        /// <summary>
        /// Called after the <see cref="Resolve"/> method was executed.
        /// </summary>
        protected virtual void AfterResolved() { }


        /// <summary>
        /// Resolve the module and all its submodules.
        /// </summary>
        public override void Resolve()
        {
            this.Resolve(null);
        }

        public override void Resolve(IRegistry registry)
        {
            if (IsResolved)
            {
                ExceptionHelper.ThrowTypeException<TModule>(Errors.InjectionModule_AlreadyResolved);
            }

            CheckAllPropertiesAreValid();

            BeforeResolving();

            try
            {
                isResolving = true;

                var usedRegistry = this.GetUsedRegistry(registry);

                ModuleResolver<IModule, TModule> resolver = new ModuleResolver<IModule, TModule>((TModule)(object)this, _container, usedRegistry);

                resolver.Resolve();

                _isResolved = true;

                BeforePostResolveAssembly();
            }
            finally
            {
                isResolving = false;
            }

            AfterResolved();
        }

        private IRegistry GetUsedRegistry(IRegistry registry)
        {
            var usedRegistry = this._registry;
            if (registry != null)
            {
                usedRegistry = this._registry.Merge(registry);
            }
            return usedRegistry;
        }

        /// <summary>
        /// Checks all properties are valid.
        /// </summary>
        /// <remarks>
        /// Properties of a module must either be:
        /// - public: declared in the module interface
        /// - private: marked with PrivateComponent on the property
        /// - non module: they are no components of the module but manually handled properties
        /// </remarks>
        private static void CheckAllPropertiesAreValid()
        {
            Type moduleType = typeof(TModule);
            Type moduleInterface = typeof(IModule);

            var publicProperties = moduleInterface.GetModuleComponentPropertiesRecursive();
            var moduleProperties = moduleType.GetModuleComponentPropertiesRecursive(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var unattributedProperties = moduleProperties.Where(
                p =>
                {
                    bool isPrivate = p.HasCustomAttribute<PrivateComponentAttribute>();
                    bool isRegistry = p.HasCustomAttribute<RegistryComponentAttribute>();
                    bool isNonModule = p.HasCustomAttribute<NonModulePropertyAttribute>();

                    return !isPrivate && !isRegistry && !isNonModule;
                });

            var publicPropertyNames = publicProperties.Select(x => x.Name);
            var unattributedPropertyNames = unattributedProperties.Select(x => x.Name);

            var invalidPropertyNames = unattributedPropertyNames.Except(publicPropertyNames);

            if (invalidPropertyNames.Any())
            {
                ExceptionHelper.ThrowTypeException<TModule>(Errors.InjectionModule_InvalidProperty, string.Join(", ", invalidPropertyNames));
            }
        }

        /// <summary>
        /// Get a component of the specified type with the specified name.
        /// </summary>
        /// <param name="componentType">The type/interface with which the component is registered in the module.</param>
        /// <param name="componentName">The name of the component.</param>
        /// <returns>
        /// If the component is found it is returned, else an exception is thrown.
        /// </returns>
        public override object GetComponent(Type componentType, string componentName)
        {
            return _container.Resolve(componentName, componentType);
        }

        /// <summary>
        /// Generic version of <see cref="GetComponent" />.
        /// </summary>
        /// <typeparam name="IComponent">The type/interface with which the component is registered in the module.</typeparam>
        /// <param name="componentName">The name of the component.</param>
        /// <returns>
        /// If the component is found it is returned, else an exception is thrown.
        /// </returns>
        public override IComponent GetComponent<IComponent>(string componentName)
        {
            return _container.Resolve<IComponent>(componentName);
        }

        /// <summary>
        /// Register a private component of the module.
        /// </summary>
        /// <typeparam name="IComponent">The interface of the component that is registered.</typeparam>
        /// <param name="moduleProperty">Expression that describes the module property where the component should be stored.</param>
        /// <returns>A context for fluent injection into the component.</returns>
        protected IRegistrationContext<IComponent, IModule, TModule>
            RegisterPrivateComponent<IComponent>(Expression<Func<TModule, IComponent>> moduleProperty)
        {
            CommonFunctions.CheckNullArgument("moduleProperty", moduleProperty);

            CheckExpressionDescribesDirectMember(moduleProperty);

            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string componentName = propInfo.Name;

            CheckPropertyQualifiesForPrivateRegistration(propInfo);

            return RegisterComponentInContainer<IComponent>(componentName);
        }


        /// <summary>
        /// Register a public component of the module.
        /// </summary>
        /// <typeparam name="IComponent">The interface of the component that is registered.</typeparam>
        /// <param name="moduleProperty">Expression that describes the module property where the component should be stored.</param>
        /// <returns>A context for fluent injection into the component.</returns>
        protected IRegistrationContext<IComponent, IModule, TModule>
            RegisterPublicComponent<IComponent>(Expression<Func<IModule, IComponent>> moduleProperty)
        {
            CommonFunctions.CheckNullArgument("moduleProperty", moduleProperty);

            CheckExpressionDescribesDirectMember(moduleProperty);

            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string componentName = propInfo.Name;

            return RegisterComponentInContainer<IComponent>(componentName);
        }

        /// <summary>
        /// Register a factory method that produces public components of the module.
        /// </summary>
        /// <typeparam name="IComponent">The interface of the component that is registered.</typeparam>
        /// <param name="moduleProperty">Expression that describes the module method that should produce the components.</param>
        /// <returns>A context for fluent injection into the component factory method.</returns>
        protected IRegistrationContext<IComponent, IModule, TModule>
            RegisterPublicComponentFactory<IComponent>(Expression<Func<IModule, IComponent>> moduleMethod)
        {
            CommonFunctions.CheckNullArgument("moduleMethod", moduleMethod);

            CheckExpressionDescribesDirectMember(moduleMethod);

            MethodCallExpression method = (MethodCallExpression)moduleMethod.Body;
            MethodInfo methodInfo = method.Method;

            return RegisterFactoryInContainer<IComponent>(methodInfo);
        }

        /// <summary>
        /// Register a factory method that produces public components of the module.
        /// </summary>
        /// <typeparam name="IComponent">The interface of the component that is registered.</typeparam>
        /// <param name="moduleProperty">Expression that describes the module method that should produce the components.</param>
        /// <returns>A context for fluent injection into the component factory method.</returns>
        protected IRegistrationContext<IComponent, IModule, TModule>
            RegisterPrivateComponentFactory<IComponent>(Expression<Func<TModule, IComponent>> moduleMethod)
        {
            CommonFunctions.CheckNullArgument("moduleMethod", moduleMethod);

            CheckExpressionDescribesDirectMember(moduleMethod);

            MethodCallExpression method = (MethodCallExpression)moduleMethod.Body;
            MethodInfo methodInfo = method.Method;

            CheckMethodQualifiesForPrivateRegistration(methodInfo);

            return RegisterFactoryInContainer<IComponent>(methodInfo);
        }

        /// <summary>
        /// Creates a component that should be returned by a factory method.
        /// </summary>
        /// <typeparam name="IComponent">The interface of the component.</typeparam>
        /// <param name="moduleMethod">The module factory method that should return the component.</param>
        /// <returns>The component that was created.</returns>
        protected IComponent CreateInstance<IComponent>(Expression<Func<TModule, IComponent>> moduleMethod)
        {
            CommonFunctions.CheckNullArgument("moduleMethod", moduleMethod);

            MethodCallExpression method = (MethodCallExpression)moduleMethod.Body;
            MethodInfo methodInfo = method.Method;
            string functionName = methodInfo.Name;

            // this must be available during resolution now, because of lambda expression
            if (!isResolving && !IsResolved)
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_CreateInstanceBeforeResolve, functionName);
            }

            if (!_container.IsRegistered<IComponent>(functionName))
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_FactoryMethodNotRegistered, functionName);
            }

            return _container.Resolve<IComponent>(functionName);
        }
        private RegistrationContext<IComponent, IModule, TModule> RegisterComponentInContainer<IComponent>(string componentName)
        {
            RegistrationContext<IComponent, IModule, TModule> registrationContext;
            _container.SetLifetime<IComponent>(componentName, new ComponentLifetime(this));

            RegistrationTypes types = CreateTypes<IComponent>();
            RegistrationContext context = new RegistrationContext(componentName, this, _container, types, false);
            registrationContext = new RegistrationContext<IComponent, IModule, TModule>(context);
            return registrationContext;
        }

        private IRegistrationContext<IComponent, IModule, TModule> RegisterFactoryInContainer<IComponent>(MethodInfo methodInfo)
        {
            string functionName = methodInfo.Name;
            if (methodInfo.GetParameters().Length > 0)
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_FactoryMethodsWithParametersNotSupportedYet, functionName);
            }

            _container.SetLifetime<IComponent>(functionName, new FactoryLifetime(this));

            var types = CreateTypes<IComponent>();

            RegistrationContext context = new RegistrationContext(functionName, this, this.Container, types, false);
            return new RegistrationContext<IComponent, IModule, TModule>(context);
        }
        
        private static void CheckExpressionDescribesDirectMember<TObject, IComponent>(Expression<Func<TObject, IComponent>> moduleProperty)
        {
            int depth;
            string path = LinqHelper.GetMemberPath(moduleProperty, out depth);
            if (depth > 1)
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_CannotRegisterPropertyOrMethodsWhichAreNotMembersOfTheModule, path);
            }

            ParameterExpression parameterExpression = moduleProperty.Parameters[0];

            ParameterExpression paramExp = null;
            MemberExpression propExpression = moduleProperty.Body as MemberExpression;
            MethodCallExpression methodExpression = moduleProperty.Body as MethodCallExpression;
            if (propExpression != null)
            {
                PropertyInfo propInfo = propExpression.Member as PropertyInfo;
                if (propInfo == null || !IsTypeOfModule(propInfo.DeclaringType))
                {
                    ThrowNoPropertyOrMethodOfModuleException(moduleProperty);
                }
                paramExp = CommonLinq.GetParameterExpressionWithPossibleConvert(propExpression.Expression, parameterExpression.Type);
            }
            else if (methodExpression != null)
            {
                MethodInfo methodInfo = methodExpression.Method;
                if (!IsTypeOfModule(methodInfo.DeclaringType))
                {
                    ThrowNoPropertyOrMethodOfModuleException(moduleProperty);
                }
                paramExp = CommonLinq.GetParameterExpressionWithPossibleConvert(methodExpression.Object, parameterExpression.Type);
            }
            else
            {
                ThrowNoPropertyOrMethodOfModuleException(moduleProperty);
            }

            if (paramExp == null || !IsTypeOfModule(paramExp.Type))
            {
                ThrowNoPropertyOrMethodOfModuleException(moduleProperty);
            }
        }

        private static void ThrowNoPropertyOrMethodOfModuleException<TObject, IComponent>(Expression<Func<TObject, IComponent>> expression)
        {
            ExceptionHelper.ThrowTypeException<TModule>(Errors.InjectionModule_NeitherPropertyNorMethodExpression, expression);
        }

        private static bool IsTypeOfModule(Type type)
        {
            return type.IsAssignableFrom(typeof(IModule)) || type.IsAssignableFrom(typeof(TModule));
        }

        private static void CheckPropertyQualifiesForPrivateRegistration(MemberInfo propInfo)
        {
            string propName = propInfo.Name;
            var isInterfaceProperty = typeof(IModule).GetProperty(propName) != null;

            if (isInterfaceProperty || !propInfo.HasCustomAttribute<PrivateComponentAttribute>())
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_PropertyNotQualifiedForPrivateRegistration, propName);
            }
        }

        private static void CheckMethodQualifiesForPrivateRegistration(MethodInfo methodInfo)
        {
            string methodName = methodInfo.Name;
            var isInterfaceProperty = typeof(IModule).GetProperty(methodName) != null;

            if (isInterfaceProperty || !methodInfo.HasCustomAttribute<PrivateFactoryAttribute>())
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_MethodNotQualifiedForPrivateRegistration, methodName);
            }
        }

        private static RegistrationTypes CreateTypes<IComponent>()
        {
            return new RegistrationTypes()
            {
                IComponent = typeof(IComponent),
                IModule = typeof(IModule),
                TModule = typeof(TModule)
            };
        }

        internal override void OnComponentResolved(ObjectResolvedContext context)
        {
            Type moduleType = typeof(TModule);

            moduleType.SetPropertyRecursive(this, context.Name, context.Instance, true);
        }

        #region IDisposable

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _container.Dispose();
            _registry.Dispose();
        }

        #endregion
    }
}
