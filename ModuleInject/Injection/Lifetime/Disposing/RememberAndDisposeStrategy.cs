using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Disposing;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection
{
    public class RememberAndDisposeStrategy : DisposeStrategy
    {
        public override void OnInstance(object instance)
        {
            this.AddInstance(instance);
        }
    }
}
