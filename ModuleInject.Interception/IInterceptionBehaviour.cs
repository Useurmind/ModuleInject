using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleInject.Interception
{
    public interface IInterceptionBehaviour
    {
        InterceptedCallResult Execute(InterceptedCallInformation callInformation, Func<InterceptedCallResult> executeNextBehaviour);
    }
}
