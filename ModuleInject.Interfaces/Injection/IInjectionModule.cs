using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    public interface IInjectionModule
    {
        void RegisterInjectionRegister(IInjectionRegister injectionRegister);
    }
}
