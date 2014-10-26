﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class ChangeReturnValueBehaviour : IInterceptionBehavior
    {
        public static int ReturnValue { get; private set; }

        static ChangeReturnValueBehaviour()
        {
            ReturnValue = 6;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            getNext().Invoke(input, getNext);
            return input.CreateMethodReturn(ReturnValue, null);
        }


        public bool WillExecute
        {
            get
            {
                return true;
            }
        }
    }
}
