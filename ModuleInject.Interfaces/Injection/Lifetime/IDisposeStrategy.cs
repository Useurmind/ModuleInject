using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// Strategy that is responsible for disposing a component.
    /// Dispose is called when the module/injection register is disposed.
    /// </summary>
    public interface IDisposeStrategy : IDisposable
    {
        /// <summary>
        /// Called when an instance of the component is created.
        /// </summary>
        /// <param name="instance">The created instance.</param>
        void OnInstance(object instance);
    }
}
