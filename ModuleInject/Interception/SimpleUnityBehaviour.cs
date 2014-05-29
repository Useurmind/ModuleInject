using Unity = Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interception
{
    internal class SimpleUnityBehaviour<TBehaviour> : Unity.IInterceptionBehavior
        where TBehaviour : ISimpleBehaviour, new()
    {
        public ISimpleBehaviour Behaviour { get; private set; }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Behaviour.GetRequiredInterfaces();
        }

        public bool WillExecute
        {
            get { return Behaviour.WillExecute; }
        }

        public SimpleUnityBehaviour()
        {
            Behaviour = new TBehaviour();
        }

        public Unity.IMethodReturn Invoke(Unity.IMethodInvocation input, Unity.GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodInvocation invocation = new MethodInvocation(input);

            IMethodReturn beforeMethodReturn = Behaviour.BeforeMethodCall(invocation);
            if (beforeMethodReturn != null)
            {
                return ((MethodReturn)beforeMethodReturn).UnityMethodReturn;
            }

            Unity.IMethodReturn actualMethodReturn = getNext()(input, getNext);
            IMethodReturn actualMethodReturnWrapped = new MethodReturn(actualMethodReturn);

            if (actualMethodReturn.Exception != null)
            {
                IMethodReturn exceptionMethodReturn = Behaviour.OnExceptionThrown(invocation, actualMethodReturnWrapped);
                if (exceptionMethodReturn != null)
                {
                    return ((MethodReturn)exceptionMethodReturn).UnityMethodReturn;
                }
            }
            else
            {
                IMethodReturn afterMethodReturn = Behaviour.AfterMethodCall(invocation, actualMethodReturnWrapped);
                if (afterMethodReturn != null)
                {
                    return ((MethodReturn)afterMethodReturn).UnityMethodReturn;
                }
            }

            return actualMethodReturn;
        }
    }
}
