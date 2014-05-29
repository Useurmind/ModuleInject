using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;

namespace ModuleInject.Interception
{
    internal class MethodInvocation : IMethodInvocation
    {
        internal Unity.IMethodInvocation UnityMethodInvocation { get; private set; }

        public MethodInvocation(Unity.IMethodInvocation invocation)
        {
            UnityMethodInvocation = invocation;
            Arguments = new ParameterCollection(UnityMethodInvocation.Arguments);
            Inputs = new ParameterCollection(UnityMethodInvocation.Inputs);

        }

        public IParameterCollection Arguments
        {
            get;
            private set;
        }

        public IParameterCollection Inputs
        {
            get;
            private set;
        }

        public MethodBase MethodBase
        {
            get {
                return UnityMethodInvocation.MethodBase;
            }
        }

        public object Target
        {
            get
            {
                return UnityMethodInvocation.Target;
            }
        }

        public IMethodReturn CreateExceptionMethodReturn(Exception ex)
        {
            Unity.IMethodReturn unityMethodReturn = UnityMethodInvocation.CreateExceptionMethodReturn(ex);
            return new MethodReturn(unityMethodReturn);
        }

        public IMethodReturn CreateMethodReturn(object returnValue, params object[] outputs)
        {
            Unity.IMethodReturn unityMethodReturn = UnityMethodInvocation.CreateMethodReturn(returnValue, outputs);
            return new MethodReturn(unityMethodReturn);
        }
    }
}
