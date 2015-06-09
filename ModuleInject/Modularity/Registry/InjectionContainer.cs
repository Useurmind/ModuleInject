using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Disposing;
using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Utility;
using ModuleInject.Injection;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Modularity.Registry
{
    public interface ISimpleInjectionContainer : IDisposable
    {
        bool IsRegistered(Type type);

        object GetComponent(Type type);

        void Register(Type type, Func<object> factoryFunc);
    }

    public interface IInjectionContainer : IDisposable
    {
        bool IsRegistered(Type type, string name);

        object GetComponent(Type type, string name);

        void Register(Type type, string name, Func<object> factoryFunc);
    }

    public class InjectionContainer : DisposableExtBase, ISimpleInjectionContainer, IInjectionContainer
    {
        private DoubleKeyDictionary<Type, string, IInjectionRegister> registeredComponents;

        public InjectionContainer()
        {
            registeredComponents = new DoubleKeyDictionary<Type, string, IInjectionRegister>();
        }

        public object GetComponent(Type type)
        {
            return GetComponent(type, string.Empty);
        }

        public object GetComponent(Type type, string name)
        {
            IInjectionRegister injectionRegister = null;
            if(!registeredComponents.TryGetValue(type, name, out injectionRegister))
            {
                ExceptionHelper.ThrowFormatException(Errors.InjectionContainer_ComponentNotRegistered, name, type);
            }

            return injectionRegister.GetInstance();
        }

        public bool IsRegistered(Type type)
        {
            return IsRegistered(type, string.Empty);
        }

        public bool IsRegistered(Type type, string name)
        {
            return registeredComponents.Contains(type, name);
        }

        public void Register(Type type, Func<object> factoryFunc)
        {
            this.Register(type, string.Empty, factoryFunc);
        }

        public void Register(Type type, string name, Func<object> factoryFunc)
        {
            if (this.registeredComponents.Contains(type, name))
            {
                ExceptionHelper.ThrowFormatException(Errors.InjectionContainer_ComponentAlreadyRegistered, name, type);
            }

            var injectionRegister = new InjectionRegister(name, typeof(object), type, type);

            injectionRegister.InstantiationStrategy(new SingleInstanceInstantiationStrategy());
            injectionRegister.DisposeStrategy(new RememberAndDisposeStrategy());
            injectionRegister.Construct(ctx => factoryFunc());

            this.registeredComponents.Add(type, name, injectionRegister);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                foreach (var injectionRegister in registeredComponents.GetAll())
                {
                    injectionRegister.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
