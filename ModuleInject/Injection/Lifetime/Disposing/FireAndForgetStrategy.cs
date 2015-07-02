using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Disposing;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection
{
    /// <summary>
    /// Does nothing in fact. You could even state null instead of it...
    /// </summary>
    public class FireAndForgetStrategy : IDisposeStrategy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Nothing happens here.")]
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public void OnInstance(object instance)
        {
        }
    }
}
