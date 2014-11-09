using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject
{
    using NUnit.Framework;
    using Test.ModuleInject.TestModules;

    [TestFixture]
    class ModifyInstancesModuleTest
    {
        private ModifyInstancesModule module;

        [SetUp]
        public void Setup()
        {
            this.module = new ModifyInstancesModule();
        }

        [TestCase]
        public void Resolved_AfterRegisteringComponentAndModifyingSubmodulePropertyInjectedIntoProperty_PropertyOfSubComponentSetByModification()
        {
            this.module.RegisterComponentAndModifySubModulePropertyOnPropertyInjection();
            this.module.Resolve();

            Assert.AreSame(this.module.Component.SubComponent1.SubComponent2, this.module.SubComponent);
        }

        [TestCase]
        public void Resolved_AfterRegisteringInstanceAndModifyingSubmodulePropertyInjectedIntoProperty_PropertyOfSubComponentSetByModification()
        {
            this.module.RegisterInstanceAndModifySubModulePropertyOnPropertyInjection();
            this.module.Resolve();

            Assert.AreSame(this.module.Component.SubComponent1.SubComponent2, this.module.SubComponent);
        }

        [TestCase]
        public void Resolved_AfterRegisterComponentAndModifySubModulePropertyOnStrongInterfaceInjectorInjection_PropertyOfSubComponentSetByModification()
        {
            this.module.RegisterComponentAndModifySubModulePropertyOnStrongInterfaceInjectorInjection();
            this.module.Resolve();

            Assert.AreSame(this.module.Component.SubComponent1.SubComponent2, this.module.SubComponent);
        }

        [TestCase]
        public void Resolved_AfterRegisterComponentAndModifySubModulePropertyOnWeakInterfaceInjectorInjection_PropertyOfSubComponentSetByModification()
        {
            this.module.RegisterComponentAndModifySubModulePropertyOnWeakInterfaceInjectorInjection();
            this.module.Resolve();

            Assert.AreSame(this.module.Component.SubComponent1.SubComponent2, this.module.SubComponent);
        }

        [TestCase]
        public void Resolved_AfterRegisterComponentAndModifySubModulePropertyOnConstructorInjection_PropertyOfComponentsMainComponent2SetToIntValue()
        {
            this.module.RegisterComponentAndModifySubModulePropertyOnConstructorInjection();
            this.module.Resolve();

            Assert.AreEqual(this.module.IntValue, this.module.Component.MainComponent2.IntProperty);
        }

        [TestCase]
        public void Resolved_AfterRegisterComponentAndModifySubModulePropertyOnMethodInjection_PropertyOfComponentsMainComponent2SetToIntValue()
        {
            this.module.RegisterComponentAndModifySubModulePropertyOnMethodInjection();
            this.module.Resolve();

            Assert.AreEqual(this.module.IntValue, this.module.Component.MainComponent2.IntProperty);
        }
    }
}
