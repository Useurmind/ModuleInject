using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interception
{
    public delegate InvokeMethodDelegate GetNextMethodInvocationDelegate();
    public delegate IMethodReturn InvokeMethodDelegate(IMethodInvocation methodInvocation, GetNextMethodInvocationDelegate getNext);

}
