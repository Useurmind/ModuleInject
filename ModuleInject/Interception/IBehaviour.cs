using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interception
{
    public interface IBehaviour
    {
        bool WillExecute { get; set; }

        IEnumerable<Type> GetRequiredInterfaces();

        IMethodReturn Invoke(IMethodInvocation methodInvocation, GetNextMethodInvocationDelegate getNext);
    }
}
