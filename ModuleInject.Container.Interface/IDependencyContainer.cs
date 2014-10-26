using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Interface
{
    public interface IDependencyContainer : IDisposable
    {
        object Resolve(string name, Type type);

        bool IsRegistered(string name, Type type);

        void Register(string name, Type registeredType, Type actualType);

        void Register(string name, Type registeredType, object instance);

        void SetLifetime(string name, Type type, ILifetime lifetime);

        void InjectProperty(string name, Type type, string propertyName, IResolvedValue value);

        void InjectMethod(string name, Type type, string methodName, IEnumerable<IResolvedValue> resolvedValues);

        void InjectConstructor(string name, Type type, IEnumerable<IResolvedValue> resolvedValue);

        void Register(string name, Type registeredType, Func<IDependencyContainer, object> createInstance);

        void AddBehaviour(string name, Type type, IInterceptionBehavior behaviour);
    }
}
