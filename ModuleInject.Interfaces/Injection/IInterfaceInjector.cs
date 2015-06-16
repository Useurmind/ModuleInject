using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    public interface IInterfaceInjector<TIContext, TIComponent>
    {
        void InjectInto(IInterfaceModificationContext<TIContext, TIComponent> injectionRegister);
    }

    public interface IExactInterfaceInjector<TIContext, TIComponent>
    {
        void InjectInto(IExactInterfaceModificationContext<TIContext, TIComponent> injectionRegister);
    }
}
