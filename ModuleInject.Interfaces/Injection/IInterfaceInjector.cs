using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    public interface IInterfaceInjector<TIContext, TIComponent>
    {
        void InjectInto(IInterfaceInjectionRegister<TIContext, TIComponent> injectionRegister);
    }

    public interface IExactInterfaceInjector<TIContext, TIComponent>
    {
        void InjectInto(IExactInterfaceInjectionRegister<TIContext, TIComponent> injectionRegister);
    }
}
