using Unity = Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interception
{
    internal class UnityBehaviour : Unity.IInterceptionBehavior
    {
        public IBehaviour Behaviour { get; set; }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Behaviour.GetRequiredInterfaces();
        }

        public Unity.IMethodReturn Invoke(Unity.IMethodInvocation input, Unity.GetNextInterceptionBehaviorDelegate getNext)
        {
            return input.CreateMethodReturn(null, null);
            //IMethodInvocation invocation = new MethodInvocation(input);
            //GetNextMethodInvocationDelegate getNext2 = () =>
            //{
            //    Unity.InvokeInterceptionBehaviorDelegate invoke = getNext();
            //    InvokeMethodDelegate invoke2 = () =>
            //    {
            //    };
            //    return invoke2;
            //};
            //IMethodReturn returnValue = Behaviour.Invoke(invocation, getNext2);
        }

        public bool WillExecute
        {
            get { return Behaviour.WillExecute; }
        }
    }
}
