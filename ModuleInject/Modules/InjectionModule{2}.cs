using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Linq;
using ModuleInject.Common.Utility;
using ModuleInject.Container;
using ModuleInject.Container.Interface;
using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;
using ModuleInject.Modularity.Registry;
using ModuleInject.Modules.Fluent;
using ModuleInject.Utility;
using System.Collections.Generic;

namespace ModuleInject.Modules
{
    /// <summary>
    /// Base class for all modules that you want to build.
    /// </summary>
    /// <typeparam name="IModule">The interface of the module.</typeparam>
    /// <typeparam name="TModule">The type of the module.</typeparam>
    public abstract class InjectionModule<IModule, TModule> : InjectionModule
        where TModule : InjectionModule<IModule, TModule>, IModule
        where IModule : Interfaces.IModule
    {
        private IList<RegistrationContext> registrationContexts;
        private IDependencyContainer container;
		private ModuleMemberExpressionChecker<IModule, TModule> memberExpressionChecker;

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
                return this.container;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectionModule{IModule, TModule}"/> class.
        /// </summary>
        protected InjectionModule()
        {
			this.memberExpressionChecker = new ModuleMemberExpressionChecker<IModule, TModule>();
            this.registrationContexts = new List<RegistrationContext>();
            this.container = new DependencyContainer();

            if (!typeof(IModule).IsInterface)
            {
                ExceptionHelper.ThrowTypeException<IModule>(Errors.InjectionModule_ModulesMustHaveAnInterface);
            }
        }

        /// <inheritdoc />
        protected override void OnRegistryResolved(IRegistry usedRegistry)
        {
            CheckAllPropertiesAreValid();

            var resolver = new ModuleResolver<IModule, TModule>((TModule)(object)this, this.container, usedRegistry);

            resolver.CheckBeforeResolve();

            resolver.ResolveSubmodules();

            resolver.ResolveComponents();
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
        public object GetComponent(Type componentType, string componentName)
        {
            return this.container.Resolve(componentName, componentType);
        }

        /// <summary>
        /// Generic version of <see cref="GetComponent" />.
        /// </summary>
        /// <typeparam name="IComponent">The type/interface with which the component is registered in the module.</typeparam>
        /// <param name="componentName">The name of the component.</param>
        /// <returns>
        /// If the component is found it is returned, else an exception is thrown.
        /// </returns>
        public IComponent GetComponent<IComponent>(string componentName)
        {
            return this.container.Resolve<IComponent>(componentName);
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

			memberExpressionChecker.CheckExpressionDescribesDirectMemberAndGetMemberName(moduleProperty);

            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string componentName = propInfo.Name;

            CheckPropertyQualifiesForPrivateRegistration(propInfo);

            return this.RegisterComponentInContainer<IComponent>(componentName);
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

			memberExpressionChecker.CheckExpressionDescribesDirectMemberAndGetMemberName(moduleProperty);

            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string componentName = propInfo.Name;

            return this.RegisterComponentInContainer<IComponent>(componentName);
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

			memberExpressionChecker.CheckExpressionDescribesDirectMemberAndGetMemberName(moduleMethod);

            MethodCallExpression method = (MethodCallExpression)moduleMethod.Body;
            MethodInfo methodInfo = method.Method;

            return this.RegisterFactoryInContainer<IComponent>(methodInfo);
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

			memberExpressionChecker.CheckExpressionDescribesDirectMemberAndGetMemberName(moduleMethod);

            MethodCallExpression method = (MethodCallExpression)moduleMethod.Body;
            MethodInfo methodInfo = method.Method;

            CheckMethodQualifiesForPrivateRegistration(methodInfo);

            return this.RegisterFactoryInContainer<IComponent>(methodInfo);
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
            if (!this.IsResolving && !this.IsResolved)
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_CreateInstanceBeforeResolve, functionName);
            }

            if (!this.container.IsRegistered<IComponent>(functionName))
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_ComponentNotRegistered, functionName);
            }

            return this.container.Resolve<IComponent>(functionName);
        }

        private RegistrationContext<IComponent, IModule, TModule> RegisterComponentInContainer<IComponent>(string componentName)
        {
            RegistrationContext<IComponent, IModule, TModule> registrationContext;
            this.container.SetLifetime<IComponent>(componentName, new ComponentLifetime(this));

            RegistrationTypes types = CreateTypes<IComponent>();
            RegistrationContext context = this.CreateRegistrationContext(componentName, types);
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

            this.container.SetLifetime<IComponent>(functionName, new FactoryLifetime(this));

            var types = CreateTypes<IComponent>();

            RegistrationContext context = CreateRegistrationContext(functionName, types);
            return new RegistrationContext<IComponent, IModule, TModule>(context);
        }

        private RegistrationContext CreateRegistrationContext(string componentName, RegistrationTypes types)
        {
            var registrationContext = new RegistrationContext(componentName, this, this.Container, types, false);
            this.registrationContexts.Add(registrationContext);
            return registrationContext;
        }

        internal override IEnumerable<RegistrationContext> GetRegistrationContexts()
        {
            return this.registrationContexts;
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
            base.Dispose(disposing);

            this.container.Dispose();
        }

        #endregion
    }
}
