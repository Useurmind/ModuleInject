using ModuleInject.Interception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public class ChangeReturnValueBehaviour : ISimpleBehaviour
    {
        public static int ReturnValue { get; private set; }

        static ChangeReturnValueBehaviour()
        {
            ReturnValue = 6;
        }

        public bool WillExecute
        {
            get
            {
                return true;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn BeforeMethodCall(IMethodInvocation methodInvocation)
        {
            return null;
        }

        public IMethodReturn AfterMethodCall(IMethodInvocation methodInvocation, IMethodReturn actualMethodReturn)
        {
            return methodInvocation.CreateMethodReturn(ReturnValue);
        }

        public IMethodReturn OnExceptionThrown(IMethodInvocation methodInvocation, IMethodReturn actualMethodReturn)
        {
            throw new NotImplementedException();
        }
    }
}
