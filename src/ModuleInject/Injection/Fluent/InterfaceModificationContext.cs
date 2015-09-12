using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Disposing;

namespace ModuleInject.Injection
{
    public class InterfaceModificationContext<TIContext, TIComponent> : IInterfaceModificationContext<TIContext, TIComponent>
    {
        public InterfaceModificationContext(IInjectionRegister injectionRegister)
        {
            this.Register = injectionRegister;
        }

        public IInjectionRegister Register { get; private set; }

        public IInterfaceModificationContext<TIContext, TIComponent> Inject(Action<TIContext, TIComponent> injectInInstance)
        {
            this.Register.Inject((ctx, comp) => injectInInstance((TIContext)ctx, (TIComponent)comp));
            return this;
        }

        public IInterfaceModificationContext<TIContext, TIComponent> AddMeta<T>(T metaData)
        {
            this.Register.AddMeta(metaData);
            return this;
        }
    }
}
