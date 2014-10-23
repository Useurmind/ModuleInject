using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Interface
{
    public static class DependencyContainerExtensions
    {
        public static bool IsRegistered<T>(this IDependencyContainer container, string name)
        {
            return container.IsRegistered(name, typeof(T));
        }

        public static T Resolve<T>(this IDependencyContainer container, string name)
        {
            return (T)container.Resolve(name, typeof(T));
        }

        public static void Register<TRegistered, TActual>(this IDependencyContainer container, string name)
        {
            container.Register(name, typeof(TRegistered), typeof(TActual));
        }

        public static void Register<T>(this IDependencyContainer container, string name, T instance)
        {
            container.Register(name, typeof(T), instance);
        }

        public static void SetLifetime<T>(this IDependencyContainer container, string name, ILifetime lifetime)
        {
            container.SetLifetime(name, typeof(T), lifetime);
        }

        public static void InjectMethod(this IDependencyContainer container, string name, Type type, string methodName, params IResolvedValue[] parameters)
        {
            container.InjectMethod(name, type, methodName, parameters);
        }

        public static void InjectConstructor(this IDependencyContainer container, string name, Type type, params IResolvedValue[] parameters)
        {
            container.InjectConstructor(name, type, parameters);
        }
    }
}
