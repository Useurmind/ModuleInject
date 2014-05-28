using Microsoft.Practices.Unity;
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

        protected ComponentRegistrationContext<IComponent, TComponent, IModule> RegisterComponent<IComponent, TComponent>(Expression<Func<TModule, IComponent>> moduleProperty)
            where TComponent : IComponent
        {
            MemberExpression member =  (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;

            _container.RegisterType<IComponent, TComponent>(propInfo.Name, new ContainerControlledLifetimeManager());

            return new ComponentRegistrationContext<IComponent, TComponent, IModule>(propInfo.Name, _container);
        }

        protected InstanceRegistrationContext<IComponent, TComponent, IModule> RegisterComponent<IComponent, TComponent>(Expression<Func<TModule, IComponent>> moduleProperty, TComponent instance)
            where TComponent : IComponent
        {
            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;
            string componentName = propInfo.Name;

            _container.RegisterInstance<IComponent>(componentName, instance);

            InstanceRegistrationContext<IComponent, TComponent, IModule> instanceContext = new InstanceRegistrationContext<IComponent, TComponent, IModule>(componentName, _container);

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
