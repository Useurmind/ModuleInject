using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Disposing;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection
{
    public class FireAndForgetStrategy : IDisposeStrategy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Nothing happens here.")]
        public void Dispose()
        {
        }

        public void OnInstance(object instance)
        {
        }
    }
}
