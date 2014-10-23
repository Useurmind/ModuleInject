﻿using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    using global::ModuleInject.Common.Exceptions;

    [TestFixture]
    public class BehaviourModuleTest
    {
        private BehaviourModule _module;

        [SetUp]
        public void Init()
        {
            _module = new BehaviourModule();
        }

        [TestCase]
        public void CallIntFunctionReturns5_OnComponentWithChangeReturnBehaviour_ReturnsChangedReturnValue()
        {
            _module.RegisterBehaviour();
            _module.Resolve();

            var result = _module.InterceptedWithChangeReturnValueComponent.FunctionReturns5();

            Assert.AreEqual(ChangeReturnValueBehaviour.ReturnValue, result);
        }
    }
}
