using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;

namespace ModuleInject.Interception
{
    public interface IMethodReturn
    {
        Exception Exception { get; set; }
        IParameterCollection Outputs { get; }
        object ReturnValue { get; set; }
    }
}
