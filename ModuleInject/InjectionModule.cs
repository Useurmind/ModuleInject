using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject
{
    public abstract class InjectionModule<IModule, TModule> : IInjectionModule
        where TModule : InjectionModule<IModule, TModule>, IModule
        where IModule : IInjectionModule
    {
        private IUnityContainer _container;

        public InjectionModule()
        {
            _container = new UnityContainer();
        }

        protected ComponentRegistrationContext<IComponent, TComponent, IModule> RegisterComponent<IComponent, TComponent>(Expression<Func<TModule, IComponent>> moduleProperty)
            where TComponent : IComponent
        {
            MemberExpression member =  (MemberExpression)moduleProperty.Body;
            MemberInfo propInfo = member.Member;

            _container.RegisterType<IComponent, TComponent>(propInfo.Name, new ContainerControlledLifetimeManager());

            return new ComponentRegistrationContext<IComponent, TComponent, IModule>(propInfo.Name, _container);
        }

        public void Resolve()
        {
            ModuleResolver resolver = new ModuleResolver();
            resolver.Resolve<IModule>((IModule)(object)this, _container);
        }
    }
}
