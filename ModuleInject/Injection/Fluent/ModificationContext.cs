using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
    public class ModificationContext<TContext, TIComponent, TComponent> : ISourceOf<TIComponent>, IWrapInjectionRegister, IModificationContext<TContext, TIComponent, TComponent>
        where TComponent : TIComponent
    {
        private readonly IInjectionRegister<TContext, TIComponent, TComponent> injectionRegister;

        public IInjectionRegister Register
        {
            get
            {
                return injectionRegister.Register;
            }
        }

        public ModificationContext(IInjectionRegister<TContext, TIComponent, TComponent> injectionRegister)
        {
            this.injectionRegister = injectionRegister;
        }

        public IModificationContext<TContext, TIComponent, TComponent> Change(Func<TContext, TIComponent, TIComponent> changeInstance)
        {
            this.injectionRegister.Change(changeInstance);
            return this;
        }

        public IModificationContext<TContext, TIComponent, TComponent> Inject(Action<TContext, TComponent> injectInInstance)
        {
            this.injectionRegister.Inject(injectInInstance);
            return this;
        }

        public IModificationContext<TContext, TIComponent, TComponent> AddMeta<T>(T metaData)
        {
            this.injectionRegister.AddMeta(metaData);

            return this;
        }

        public TIComponent Get()
        {
            return injectionRegister.GetInstance();
        }
    }

    public static class SourceOfExtensions
    {
        public static IModificationContext<TContext, TIComponent, TComponent> AddInjector<TContext, TIComponent, TComponent, TIContext, TIComponent2>(
            this IModificationContext<TContext, TIComponent, TComponent> source,
            IInterfaceInjector<TIContext, TIComponent2> injector)
        where TComponent : TIComponent, TIComponent2
            where TContext : TIContext
        {
            var interfaceInjectionRegister = new InterfaceModificationContext<TIContext, TIComponent2>(source.Register);
            injector.InjectInto(interfaceInjectionRegister);
            return source;
        }

        public static IModificationContext<TContext, TIComponent, TComponent> AddInjector<TContext, TIComponent, TComponent, TIContext>(
            this IModificationContext<TContext, TIComponent, TComponent> source,
            IExactInterfaceInjector<TIContext, TIComponent> injector)
        where TComponent : TIComponent
            where TContext : TIContext
        {
            var interfaceInjectionRegister = new ExactInterfaceModificationContext<TIContext, TIComponent>(source.Register);
            injector.InjectInto(interfaceInjectionRegister);
            return source;
        }
    }
}
