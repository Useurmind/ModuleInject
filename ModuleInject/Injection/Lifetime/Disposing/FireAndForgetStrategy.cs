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
        public void Dispose()
        {
        }

        public void OnInstance(object instance)
        {
        }
    }
}
