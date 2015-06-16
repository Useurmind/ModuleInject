using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Disposing;

namespace ModuleInject.Injection
{
    public class ExactInterfaceModificationContext<TIContext, TIComponent> : 
        IExactInterfaceModificationContext<TIContext, TIComponent>
    {
        public ExactInterfaceModificationContext(IInjectionRegister injectionRegister)
        {
            this.Register = injectionRegister;
        }

        public IInjectionRegister Register { get; private set; }

        public IExactInterfaceModificationContext<TIContext, TIComponent> Inject(Action<TIContext, TIComponent> injectInInstance)
        {
            this.Register.Inject((ctx, comp) => injectInInstance((TIContext)ctx, (TIComponent)comp));
            return this;
        }

        public IExactInterfaceModificationContext<TIContext, TIComponent> Change(Func<TIComponent, TIComponent> changeInstance)
        {
            this.Register.Change(comp => changeInstance((TIComponent)comp));
            return this;
        }

        public IExactInterfaceModificationContext<TIContext, TIComponent> Change(Func<TIContext, TIComponent, TIComponent> changeInstance)
        {
            this.Register.Change((ctx, comp) => changeInstance((TIContext)ctx, (TIComponent)comp));
            return this;
        }

        public IExactInterfaceModificationContext<TIContext, TIComponent> AddMeta<T>(T metaData)
        {
            this.Register.AddMeta(metaData);
            return this;
        }
    }
}
