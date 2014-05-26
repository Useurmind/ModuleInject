using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
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
        private IUnityContainer _container;

        public InjectionModule()
        {
            _container = new UnityContainer();
        }

        protected void ActivateInterception()
        {
            _container.AddNewExtension<Interception>();
        }

        protected ComponentRegistrationContext<IComponent, TComponent, IModule> RegisterComponent<IComponent, TComponent>(Expression<Func<TModule, IComponent>> moduleProperty)
            where TComponent : IComponent
        {
            MemberExpression member =  (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;

            _container.RegisterType<IComponent, TComponent>(propInfo.Name, new ContainerControlledLifetimeManager());

            return new ComponentRegistrationContext<IComponent, TComponent, IModule>(propInfo.Name, _container);
        }

        protected ComponentRegistrationContext<IComponent, TComponent, IModule> RegisterComponent<IComponent, TComponent>(Expression<Func<TModule, IComponent>> moduleProperty, TComponent instance)
            where TComponent : IComponent
        {
            MemberExpression member = (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;

            _container.RegisterInstance<IComponent>(propInfo.Name, instance);

            return new ComponentRegistrationContext<IComponent, TComponent, IModule>(propInfo.Name, _container);
        }

        public void Resolve()
        {
            ModuleResolver resolver = new ModuleResolver();
            resolver.Resolve<IModule>((IModule)(object)this, _container);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _container.Dispose();
        }
    }
}
