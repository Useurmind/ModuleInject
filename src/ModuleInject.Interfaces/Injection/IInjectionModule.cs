using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// This interface is actually not intended to be used outside of module inject.
    /// Used by context classes to interact with the module.
    /// </summary>
    public interface IInjectionModule
    {
        void RegisterInjectionRegister(IInjectionRegister injectionRegister);
    }
}
