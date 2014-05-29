using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;

namespace ModuleInject.Interception
{
    internal class MethodReturn : IMethodReturn
    {
        internal Unity.IMethodReturn UnityMethodReturn { get; private set; }

        public MethodReturn(Unity.IMethodReturn methodReturn)
        {
            UnityMethodReturn = methodReturn;
            Outputs = new ParameterCollection(UnityMethodReturn.Outputs);
        }

        public Exception Exception
        {
            get
            {
                return UnityMethodReturn.Exception;
            }
            set
            {
                UnityMethodReturn.Exception = value;
            }
        }

        public IParameterCollection Outputs
        {
            get;
            private set;
        }

        public object ReturnValue
        {
            get
            {
                return UnityMethodReturn.ReturnValue;
            }
            set
            {
                UnityMethodReturn.ReturnValue = value;
            }
        }
    }
}
