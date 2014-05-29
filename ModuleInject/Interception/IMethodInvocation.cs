using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Interception
{
    public interface IMethodInvocation
    {
        IParameterCollection Arguments { get; }
        IParameterCollection Inputs { get; }
        MethodBase MethodBase { get; }
        object Target { get; }
        IMethodReturn CreateExceptionMethodReturn(Exception ex);
        IMethodReturn CreateMethodReturn(object returnValue, params object[] outputs);
    }
}
