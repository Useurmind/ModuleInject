using ModuleInject.Interception;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;

namespace Test.ModuleInject.Interception
{
    [TestFixture]
    public class SimpleUnityBehaviourTest
    {
        #region stubs for test

        public class UnityMethodReturn : Unity.IMethodReturn
        {
            private System.Exception _exception;
            private object _returnValue;

            public Exception Exception
            {
                get
                {
                    return _exception;
                }
                set
                {
                    _exception = value;
                }
            }

            public IDictionary<string, object> InvocationContext
            {
                get { throw new NotImplementedException(); }
            }

            public Unity.IParameterCollection Outputs
            {
                get { return null; }
            }

            public object ReturnValue
            {
                get
                {
                    return _returnValue;
                }
                set
                {
                    _returnValue = value;
                }
            }
        }

        public class UnityMethodInvocation : Unity.IMethodInvocation
        {

            public Unity.IParameterCollection Arguments
            {
                get { return null; }
            }

            public Unity.IMethodReturn CreateExceptionMethodReturn(Exception ex)
            {
                return new UnityMethodReturn() { Exception = ex };
            }

            public Unity.IMethodReturn CreateMethodReturn(object returnValue, params object[] outputs)
            {
                return new UnityMethodReturn() { ReturnValue = returnValue};
            }

            public Unity.IParameterCollection Inputs
            {
                get { return null; }
            }

            public IDictionary<string, object> InvocationContext
            {
                get { throw new NotImplementedException(); }
            }

            public System.Reflection.MethodBase MethodBase
            {
                get { return null; }
            }

            public object Target
            {
                get { return null; }
            }
        }

        public class TestBehaviour : ISimpleBehaviour
        {
            public bool BeforeCallEntered { get; set; }
            public bool AfterCallEntered { get; set; }
            public bool ExceptionEntered { get; set; }

            public bool ReturnBeforeCall { get; set; }
            public bool ReturnAfterCall { get; set; }

            public IMethodReturn MethodReturn { get; set; }

            public bool WillExecute
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerable<Type> GetRequiredInterfaces()
            {
                throw new NotImplementedException();
            }

            public IMethodReturn BeforeMethodCall(IMethodInvocation methodInvocation)
            {
                BeforeCallEntered = true;
                if (ReturnBeforeCall)
                {
                    MethodReturn = methodInvocation.CreateMethodReturn(null);
                    return MethodReturn;
                }

                return null;
            }

            public IMethodReturn AfterMethodCall(IMethodInvocation methodInvocation, IMethodReturn actualMethodReturn)
            {
                AfterCallEntered = true;
                if (ReturnAfterCall)
                {
                    MethodReturn = methodInvocation.CreateMethodReturn(null);
                    return MethodReturn;
                }

                return null;
            }

            public IMethodReturn OnExceptionThrown(IMethodInvocation methodInvocation, IMethodReturn actualMethodReturn)
            {
                ExceptionEntered = true;
                if (ReturnAfterCall)
                {
                    MethodReturn = methodInvocation.CreateMethodReturn(null);
                    return MethodReturn;
                }

                return null;
            }
        }

        #endregion


        private SimpleUnityBehaviour<TestBehaviour> _unityBehaviour;
        private TestBehaviour _behaviour;
        private Unity.GetNextInterceptionBehaviorDelegate _getNext;
        private UnityMethodInvocation _methodInvocation;
        private UnityMethodReturn _methodReturn;

        [SetUp]
        public void Init()
        {
            _unityBehaviour = new SimpleUnityBehaviour<TestBehaviour>();
            _behaviour = (TestBehaviour)_unityBehaviour.Behaviour;
            _methodReturn = new UnityMethodReturn();
            _getNext = () => (Unity.IMethodInvocation invoke, Unity.GetNextInterceptionBehaviorDelegate getNext) => _methodReturn;
            _methodInvocation = new UnityMethodInvocation();
        }

        [TestCase]
        public void Invoke_NoInterception_BeforeAndAfterEntered()
        {
            var result = _unityBehaviour.Invoke(_methodInvocation, _getNext);

            Assert.IsTrue(_behaviour.BeforeCallEntered);
            Assert.IsTrue(_behaviour.AfterCallEntered);
            Assert.IsFalse(_behaviour.ExceptionEntered);
            Assert.AreSame(_methodReturn, result);
        }

        [TestCase]
        public void Invoke_NoInterceptionButException_BeforeAndExceptionEntered()
        {
            _methodReturn.Exception = new Exception();

            var result = _unityBehaviour.Invoke(_methodInvocation, _getNext);

            Assert.IsTrue(_behaviour.BeforeCallEntered);
            Assert.IsFalse(_behaviour.AfterCallEntered);
            Assert.IsTrue(_behaviour.ExceptionEntered);
            Assert.AreSame(_methodReturn, result);
        }

        [TestCase]
        public void Invoke_ShouldLeaveBeforeCall_OnlyBeforeEntered()
        {
            _behaviour.ReturnBeforeCall = true;

            var result = _unityBehaviour.Invoke(_methodInvocation, _getNext);

            Assert.IsTrue(_behaviour.BeforeCallEntered);
            Assert.IsFalse(_behaviour.AfterCallEntered);
            Assert.IsFalse(_behaviour.ExceptionEntered);
            Assert.AreNotSame(_methodReturn, result);
        }

        [TestCase]
        public void Invoke_ShouldLeaveAfterCall_BeforeAndAfterEntered()
        {
            _behaviour.ReturnAfterCall = true;

            var result = _unityBehaviour.Invoke(_methodInvocation, _getNext);

            Assert.IsTrue(_behaviour.BeforeCallEntered);
            Assert.IsTrue(_behaviour.AfterCallEntered);
            Assert.IsFalse(_behaviour.ExceptionEntered);
            Assert.AreNotSame(_methodReturn, result);
        }

        [TestCase]
        public void Invoke_ShouldLeaveAfterCallWithException_BeforeAndExceptionEntered()
        {
            _behaviour.ReturnAfterCall = true;
            _methodReturn.Exception = new Exception();

            var result = _unityBehaviour.Invoke(_methodInvocation, _getNext);

            Assert.IsTrue(_behaviour.BeforeCallEntered);
            Assert.IsFalse(_behaviour.AfterCallEntered);
            Assert.IsTrue(_behaviour.ExceptionEntered);
            Assert.AreNotSame(_methodReturn, result);
        }
    }
}
