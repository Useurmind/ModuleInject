using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interception
{
    public interface ISimpleBehaviour
    {
        bool WillExecute { get; set; }

        IEnumerable<Type> GetRequiredInterfaces();

        IMethodReturn BeforeMethodCall(IMethodInvocation methodInvocation);

        IMethodReturn AfterMethodCall(IMethodInvocation methodInvocation, IMethodReturn actualMethodReturn);

        IMethodReturn OnExceptionThrown(IMethodInvocation methodInvocation, IMethodReturn actualMethodReturn);
    }
}
