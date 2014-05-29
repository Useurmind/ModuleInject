using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
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
    public abstract class InjectionModule<IModule, TModule> : IInjectionModule, IDisposable
        where TModule : InjectionModule<IModule, TModule>, IModule
        where IModule : IInjectionModule
    {
        private DoubleKeyDictionary<Type, string, IInstanceRegistrationContext> _instanceRegistrations;
        private IUnityContainer _container;
        private bool _isInterceptionActive;

        public bool IsResolved { get; private set; }

        protected InjectionModule()
        {
            _isInterceptionActive = false;
            IsResolved = false;
            _container = new UnityContainer();
            _instanceRegistrations = new DoubleKeyDictionary<Type, string, IInstanceRegistrationContext>();

            if (!typeof(IModule).IsInterface)
            {
                CommonFunctions.ThrowTypeException<IModule>(Errors.InjectionModule_ModulesMustHaveAnInterface);
            }
        }

        protected void ActivateInterception()
        {
            _container.AddNewExtension<Microsoft.Practices.Unity.InterceptionExtension.Interception>();
            _isInterceptionActive = true;
        }

        protected ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            RegisterPrivateComponent<IComponent, TComponent>(Expression<Func<TModule, IComponent>> moduleProperty)
            where TComponent : IComponent
        {
            CommonFunctions.CheckNullArgument("moduleProperty", moduleProperty);

            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string propName = propInfo.Name;

            CheckPropertyQualifiesForPrivateRegistration(propInfo);

            return RegisterContainerComponent<IComponent, TComponent>(propName);
        }

        protected ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            RegisterPublicComponent<IComponent, TComponent>(Expression<Func<IModule, IComponent>> moduleProperty)
            where TComponent : IComponent
        {
            CommonFunctions.CheckNullArgument("moduleProperty", moduleProperty);

            MemberExpression member =  (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string propName = propInfo.Name;

            return RegisterContainerComponent<IComponent, TComponent>(propName);
        }

        protected InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            RegisterPrivateComponent<IComponent, TComponent>(Expression<Func<TModule, IComponent>> moduleProperty,
            TComponent instance)
            where TComponent : IComponent
        {
            CommonFunctions.CheckNullArgument("moduleProperty", moduleProperty);

            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string componentName = propInfo.Name;

            CheckPropertyQualifiesForPrivateRegistration(propInfo);

            return RegisterContainerInstance<IComponent, TComponent>(instance, componentName);
        }

        protected InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> 
            RegisterPublicComponent<IComponent, TComponent>(Expression<Func<IModule, IComponent>> moduleProperty,
            TComponent instance)
            where TComponent : IComponent
        {
            CommonFunctions.CheckNullArgument("moduleProperty", moduleProperty);

            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string componentName = propInfo.Name;

            return RegisterContainerInstance<IComponent, TComponent>(instance, componentName);
        }

        protected ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            RegisterPublicComponentFactory<IComponent, TComponent>(Expression<Func<IModule, IComponent>> moduleMethod)
            where TComponent :IComponent
        {
            CommonFunctions.CheckNullArgument("moduleMethod", moduleMethod);

            MethodCallExpression method = (MethodCallExpression)moduleMethod.Body;
            MethodInfo methodInfo = method.Method;
            string functionName = methodInfo.Name;

            if (methodInfo.GetParameters().Length > 0)
            {
                CommonFunctions.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_FactoryMethodsWithParametersNotSupportedYet, functionName);
            }

            _container.RegisterType<IComponent, TComponent>(functionName);

            return new ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>(functionName, _container, _isInterceptionActive);
        }

        protected IComponent CreateInstance<IComponent>(Expression<Func<IModule, IComponent>> moduleMethod)
        {
            CommonFunctions.CheckNullArgument("moduleMethod", moduleMethod);

            MethodCallExpression method = (MethodCallExpression)moduleMethod.Body;
            MethodInfo methodInfo = method.Method;
            string functionName = methodInfo.Name;

            if (!IsResolved)
            {
                CommonFunctions.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_CreateInstanceBeforeResolve, functionName);
            }

            if (!_container.IsRegistered<IComponent>(functionName))
            {
                CommonFunctions.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_FactoryMethodNotRegistered, functionName);
            }

            return _container.Resolve<IComponent>(functionName);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification="Should be ok that way.")]
        private ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            RegisterContainerComponent<IComponent, TComponent>(string propName) where TComponent : IComponent
        {
            _container.RegisterType<IComponent, TComponent>(propName, new ContainerControlledLifetimeManager());

            return new ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>(propName, _container, _isInterceptionActive);
        }

        private InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> RegisterContainerInstance<IComponent, TComponent>(TComponent instance, string componentName) where TComponent : IComponent
        {
            _container.RegisterInstance<IComponent>(componentName, instance);

            var instanceContext = new InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>(componentName, _container);

            _instanceRegistrations.Add(typeof(IComponent), componentName, instanceContext);

            return instanceContext;
        }

        private static void CheckPropertyQualifiesForPrivateRegistration(MemberInfo propInfo)
        {
            string propName = propInfo.Name;
            var isInterfaceProperty = typeof(IModule).GetProperty(propName) != null;

            if (isInterfaceProperty || propInfo.GetCustomAttributes(typeof(PrivateComponentAttribute), false).Length == 0)
            {
                CommonFunctions.ThrowPropertyAndTypeException<TModule>(Errors.ModuleResolver_PropertyNotQualifiedForPrivateRegistration, propName);
            }
        }

        public void Resolve()
        {
            if (IsResolved)
            {
                CommonFunctions.ThrowTypeException<TModule>(Errors.InjectionModule_AlreadyResolved);
            }

            ModuleResolver.Resolve<IModule, TModule>((TModule)(object)this, _container);

            ModulePostResolveBuilder.PerformPostResolveAssembly(this, _instanceRegistrations);

            IsResolved = true;
        }

        public object GetComponent(Type componentType, string componentName)
        {
            return _container.Resolve(componentType, componentName);
        }

        public IComponent GetComponent<IComponent>(string componentName)
        {
            return _container.Resolve<IComponent>(componentName);
        }

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _container.Dispose();
        }

        #endregion
    }
}
