using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;

namespace ModuleInject.Interception
{
    internal class MethodInvocation : IMethodInvocation
    {
        private Unity.IMethodInvocation _invocation;

        public MethodInvocation(Unity.IMethodInvocation invocation)
        {
            _invocation = invocation;
        }
    }
}
