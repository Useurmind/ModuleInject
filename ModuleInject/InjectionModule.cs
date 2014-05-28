﻿using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
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

        public InjectionModule()
        {
            _container = new UnityContainer();
            _instanceRegistrations = new DoubleKeyDictionary<Type, string, IInstanceRegistrationContext>();
        }

        protected void ActivateInterception()
        {
            _container.AddNewExtension<Microsoft.Practices.Unity.InterceptionExtension.Interception>();
        }

        protected ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            RegisterPrivateComponent<IComponent, TComponent>(Expression<Func<TModule, IComponent>> moduleProperty)
            where TComponent : IComponent
        {
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
            MemberExpression member =  (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string propName = propInfo.Name;

            return RegisterContainerComponent<IComponent, TComponent>(propName);
        }

        private ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> 
            RegisterContainerComponent<IComponent, TComponent>(string propName) where TComponent : IComponent
        {
            _container.RegisterType<IComponent, TComponent>(propName, new ContainerControlledLifetimeManager());

            return new ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>(propName, _container);
        }

        private void CheckPropertyQualifiesForPrivateRegistration(MemberInfo propInfo)
        {
            string propName = propInfo.Name;
            var isInterfaceProperty = typeof(IModule).GetProperty(propName) != null;

            if (isInterfaceProperty || propInfo.GetCustomAttributes(typeof(PrivateComponentAttribute), false).Length == 0)
            {
                throw new ModuleInjectException(string.Format(Errors.ModuleResolver_PropertyNotQualifiedForPrivateRegistration,
                                                                propName, typeof(TModule).FullName));
            }
        }

        protected InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            RegisterPrivateComponent<IComponent, TComponent>(Expression<Func<TModule, IComponent>> moduleProperty,
            TComponent instance)
            where TComponent : IComponent
        {
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
            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string componentName = propInfo.Name;

            return RegisterContainerInstance<IComponent, TComponent>(instance, componentName);
        }

        private InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> RegisterContainerInstance<IComponent, TComponent>(TComponent instance, string componentName) where TComponent : IComponent
        {
            _container.RegisterInstance<IComponent>(componentName, instance);

            var instanceContext = new InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>(componentName, _container);

            _instanceRegistrations.Add(typeof(IComponent), componentName, instanceContext);

            return instanceContext;
        }

        public void Resolve()
        {
            ModuleResolver resolver = new ModuleResolver();
            resolver.Resolve<IModule, TModule>((TModule)(object)this, _container);

            ModulePostResolveBuilder instanceBuilder = new ModulePostResolveBuilder();
            instanceBuilder.PerformPostResolveAssembly(this, _instanceRegistrations);
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
