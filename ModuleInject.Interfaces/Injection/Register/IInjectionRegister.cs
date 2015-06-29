using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    public interface IInjectionRegister : IDisposable
    {
        string ComponentName { get; }
        Type ComponentInterface { get; }
        Type ComponentType { get; set; }
        Type ContextType { get; }

        IEnumerable<object> MetaData { get; }

        IInstantiationStrategy GetInstantiationStrategy();

        IDisposeStrategy GetDisposeStrategy();

        void SetContext(object context);

        void InstantiationStrategy(IInstantiationStrategy instantiationStrategy);

        void DisposeStrategy(IDisposeStrategy disposeStrategy);

        void Construct(Func<object, object> constructInstance);

        void Inject(Action<object, object> injectInInstance);

        void Change(Func<object, object, object> changeInstance);

        void Change(Func<object, object> changeInstance);

        void AddMeta(object metaData);

        void OnResolve(Action<ObjectResolvedContext> resolveHandler);

        object GetInstance();
    }

    public interface IInjectionRegister<TContext, TIComponent, TComponent> : IWrapInjectionRegister
        where TComponent : TIComponent
    {
        void SetContext(TContext context);

        void InstantiationStrategy(IInstantiationStrategy<TIComponent> instantiationStrategy);

        void DisposeStrategy(IDisposeStrategy disposeStrategy);

        void Construct(Func<TContext, TComponent> constructInstance);

        void Inject(Action<TContext, TComponent> injectInInstance);

        void Change(Func<TContext, TIComponent, TIComponent> changeInstance);

        void Change(Func<TIComponent, TIComponent> changeInstance);

        void AddMeta<T>(T metaData);

        TIComponent GetInstance();
    }
}
