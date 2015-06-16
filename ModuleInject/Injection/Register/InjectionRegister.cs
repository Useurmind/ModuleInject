using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Disposing;

namespace ModuleInject.Injection
{
    public class InjectionRegister : DisposableExtBase, IInjectionRegister
    {
        private object context;
        private IInstantiationStrategy instantiationStrategy;
        private IDisposeStrategy disposeStrategy;
        private Func<object, object> constructInstance;
        private IList<Action<object, object>> injectInInstanceList;
        private IList<Func<object, object, object>> changeInstanceList;
        private IList<Action<ObjectResolvedContext>> resolvedHandlers;
        private ISet<object> metaData;

        public InjectionRegister(string componentName, Type contextType, Type componentInterface) : this(componentName, contextType, componentInterface, null)
        {
        }

        public InjectionRegister(Type contextType, Type componentInterface, Type componentType) :this(null, contextType, componentInterface, componentType)
        {
        }

        public InjectionRegister(string componentName, Type contextType, Type componentInterface, Type componentType)
        {
            this.ComponentName = componentName;
            this.ContextType = contextType;
            this.ComponentInterface = componentInterface;
            this.ComponentType = componentType;

            this.injectInInstanceList = new List<Action<object, object>>();
            this.changeInstanceList = new List<Func<object, object, object>>();
            this.metaData = new HashSet<object>();
            this.resolvedHandlers = new List<Action<ObjectResolvedContext>>();
        }

        public string ComponentName { get; private set; }
        public Type ComponentInterface { get; private set; }
        public Type ComponentType { get; set; }
        public Type ContextType { get; private set; }

        public IEnumerable<object> MetaData { get { return this.metaData; } }
        public IInstantiationStrategy GetInstantiationStrategy()
        {
            return instantiationStrategy;
        }
        public IDisposeStrategy GetDisposeStrategy()
        {
            return disposeStrategy;
        }

        public void SetContext(object context)
        {
            this.context = context;
        }

        public void InstantiationStrategy(IInstantiationStrategy instantiationStrategy)
        {
            this.instantiationStrategy = instantiationStrategy;
        }

        public void DisposeStrategy(IDisposeStrategy disposeStrategy)
        {
            this.disposeStrategy = disposeStrategy;
        }

        public void Construct(Func<object, object> constructInstance)
        {
            this.constructInstance = constructInstance;
        }

        public void Inject(Action<object, object> injectInInstance)
        {
            this.injectInInstanceList.Add(injectInInstance);
        }

        public void Change(Func<object, object, object> changeInstance)
        {
            this.changeInstanceList.Add(changeInstance);
        }

        public void Change(Func<object, object> changeInstance)
        {
            this.Change((ctx, comp) => changeInstance(comp));
        }

        public void AddMeta(object metaData)
        {
            this.metaData.Add(metaData);
        }

        public void OnResolve(Action<ObjectResolvedContext> resolveHandler)
        {
            this.resolvedHandlers.Add(resolveHandler);
        }

        public object GetInstance()
        {
            return instantiationStrategy.GetInstance(() => CreateInstance());
        }

        private object CreateInstance()
        {
            object instance = constructInstance(this.context);
            foreach (var injectInInstance in this.injectInInstanceList)
            {
                injectInInstance(this.context, instance);
            }
            
            disposeStrategy?.OnInstance(instance);

            object usedInstance = instance;

            foreach (var changeInstance in changeInstanceList)
            {
                usedInstance = changeInstance(this.context, usedInstance);

                disposeStrategy?.OnInstance(usedInstance);
            }

            if (this.resolvedHandlers.Count > 0)
            {
                var resolvedContext = new ObjectResolvedContext()
                {
                    ContextType = this.ContextType,
                    ComponentInterface = this.ComponentInterface,
                    ComponentType = this.ComponentType,
                    Instance = usedInstance
                };
                foreach (var resolvedHandler in this.resolvedHandlers)
                {
                    resolvedHandler(resolvedContext);
                }
            }

            return usedInstance;
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {                
                disposeStrategy?.Dispose();
            }

            base.Dispose(disposing);
        }

        public override bool Equals(object obj)
        {
            var otherInjectionRegister = obj as InjectionRegister;
            if(otherInjectionRegister == null)
            {
                return false;
            }

            return otherInjectionRegister == this;
        }

        private static int nextHashCode = 0;
        private int hashCode = nextHashCode++;
        public override int GetHashCode()
        {
            return hashCode;
        }
    }

    public class InjectionRegister<TContext, TIComponent, TComponent> : IInjectionRegister<TContext, TIComponent, TComponent>
        where TComponent : TIComponent
        where TContext : class
    {
        public IInjectionRegister Register { get; private set; }

        public InjectionRegister()
        {
            this.Register = new InjectionRegister(typeof(TContext), typeof(TIComponent), typeof(TComponent));
            this.InstantiationStrategy(new FactoryInstantiationStrategy<TIComponent>());
        }

        public InjectionRegister(IInjectionRegister injectionRegister)
        {
            CheckTypes(injectionRegister);
            this.Register = injectionRegister;
            if (injectionRegister.GetInstantiationStrategy() == null)
            {
                this.InstantiationStrategy(new FactoryInstantiationStrategy<TIComponent>());
            }
        }

        private void CheckTypes(IInjectionRegister injectionRegister)
        {
            if(injectionRegister.ComponentType == null)
            {
                injectionRegister.ComponentType = typeof(TComponent);
            }

            if (injectionRegister.ContextType != typeof(TContext) ||
                injectionRegister.ComponentInterface != typeof(TIComponent) ||
                injectionRegister.ComponentType != typeof(TComponent))
            {
                var message = string.Format("The types in the inner injection register do not match the generic type arguments of the outer one: inner (({0}, {1}, {2})), generic ({3}, {4}, {5}).",
                    injectionRegister.ContextType.Name, injectionRegister.ComponentInterface.Name, injectionRegister.ComponentType.Name,
                    typeof(TContext).Name, typeof(TIComponent).Name, typeof(TComponent).Name);
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public void SetContext(TContext context)
        {
            this.Register.SetContext(context);
        }

        public void InstantiationStrategy(IInstantiationStrategy<TIComponent> instantiationStrategy)
        {
            this.Register.InstantiationStrategy(instantiationStrategy);
        }

        public void DisposeStrategy(IDisposeStrategy disposeStrategy)
        {
            this.Register.DisposeStrategy(disposeStrategy);
        }

        public void Construct(Func<TContext, TComponent> constructInstance)
        {
            this.Register.Construct(ctx => constructInstance((TContext)ctx));
            this.Register.ComponentType = typeof(TComponent);
        }

        public void Inject(Action<TContext, TComponent> injectInInstance)
        {
            this.Register.Inject((ctx, comp) => injectInInstance((TContext)ctx, (TComponent)comp));
        }

        public void Change(Func<TContext, TIComponent, TIComponent> changeInstance)
        {
            this.Register.Change((ctx, comp) => changeInstance((TContext)ctx, (TIComponent)comp));
        }

        public void Change(Func<TIComponent, TIComponent> changeInstance)
        {
            this.Change((ctx, comp) => changeInstance(comp));
        }

        public void AddMeta<T>(T metaData)
        {
            this.Register.AddMeta(metaData);
        }

        public TIComponent GetInstance()
        {
            return (TIComponent)this.Register.GetInstance();
        }
    }

    public static class InjectionRegister3Extensions
    {
        public static IInjectionRegister<TContext, TIComponent, TComponent> AddInterfaceInjector<TContext, TIComponent, TComponent, TIContext, TIComponent2>(
            this IInjectionRegister<TContext, TIComponent, TComponent> injectionRegister,
            IInterfaceInjector<TIContext, TIComponent2> injector)
            where TComponent : TIComponent, TIComponent2
            where TContext : TIContext
        {
            var interfaceInjectionRegister = new InterfaceModificationContext<TIContext, TIComponent2>(injectionRegister.Register);

            injector.InjectInto(interfaceInjectionRegister);

            return injectionRegister;
        }
    }
}
