using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ModuleInject.Interception
{
    public interface ISimpleBehaviour
    {
        bool WillExecute { get; set; }

        IEnumerable<Type> RequiredInterfaces { get; }

        IMethodReturn BeforeMethodCall(IMethodInvocation methodInvocation);

        IMethodReturn AfterMethodCall(IMethodInvocation methodInvocation, IMethodReturn actualMethodReturn);

        IMethodReturn OnExceptionThrown(IMethodInvocation methodInvocation, IMethodReturn actualMethodReturn);
    }
}
