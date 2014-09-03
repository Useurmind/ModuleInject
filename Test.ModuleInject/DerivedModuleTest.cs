﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class DerivedModuleTest
    {
        private DerivedModule _derivedModule;

        [SetUp]
        public void Setup()
        {
            _derivedModule = new DerivedModule();
        }

        [TestCase]
        public void Resolve__CorrectlyResolved()
        {
            _derivedModule.Resolve();

            Assert.IsNotNull(_derivedModule.MainComponent1);
            Assert.IsNotNull(_derivedModule.MainComponent2);
            Assert.IsNotNull(_derivedModule.MainComponent1Private);
            Assert.IsNotNull(_derivedModule.MainComponent2Private);
        }
    }
}
