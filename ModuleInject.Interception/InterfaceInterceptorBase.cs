using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModuleInject.Interception
{
    public class InterceptedCallResult
    {
        public Type ReturnValueType { get; private set; }

        public object ReturnValue { get; private set; }
    }

    public class InterceptedCallInformation
    {
        public InterceptedCallInformation(object[] arguments, string methodName, Type returnValueType)
        {
            Arguments = arguments;
            MethodName = methodName;
            ReturnValueType = returnValueType;
        }

        public object[] Arguments { get; private set; }

        public string MethodName { get; private set; }

        public Type ReturnValueType { get; private set; }
    }

    public interface IInterfaceInterceptor
    {
        void AddBehaviour(IInterceptionBehaviour behaviour);
    }

    public class InterfaceInterceptorBase<TInterceptedInterface> : IInterfaceInterceptor
    {
        private IList<IInterceptionBehaviour> behaviours;

        private TInterceptedInterface decoratedInstance;

        public InterfaceInterceptorBase()
        {
            behaviours = new List<IInterceptionBehaviour>();
        }

        public void SetInterceptedInstance(TInterceptedInterface instance)
        {
            decoratedInstance = instance;
        }

        public void AddBehaviour(IInterceptionBehaviour behaviour)
        {
            this.behaviours.Add(behaviour);
        }

        protected void ExecuteBehaviours(object[] arguments, [CallerMemberName]string memberName = null)
        {
            //var callInformation = new InterceptedCallInformation(arguments, memberName);

            //Func<InterceptedCallResult>[] behaviourExecutors = new Func<InterceptedCallResult>[this.behaviours.Count];

            //var behaviour0 = this.behaviours[0];
            //behaviourExecutors[0] = () => behaviour0.Execute(callInformation)

            //for (int i = 0; i < this.behaviours.Count; i++)
            //{
            //    var behaviour = this.behaviours[i];
            //    behaviourExecutors[i] = () => behaviour.Execute(callInformation);
            //}

            //foreach (var behaviour in this.behaviours)
            //{
            //    behaviourExecutors
            //}
        }
    }
}
