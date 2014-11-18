using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Common.Linq
{
    using global::ModuleInject.Common.Linq;

    using NUnit.Framework;
    using System.Reflection;

    [TestFixture]
    public class TypeExtensionsTest
    {
        private DerivedClass instance;

        private class BaseClass
        {
            public int IntProperty { get; private set; }
        }

        private class DerivedClass : BaseClass
        {
        }

        [SetUp]
        public void Setup()
        {
            instance = new DerivedClass();
        }

        [TestCase]
        public void Verify_StandardGetPropertyHasNoSetter()
        {
            PropertyInfo propInfo = instance.GetType().GetProperty("IntProperty");

            Assert.IsFalse(propInfo.CanWrite);
            Assert.IsNull(propInfo.GetSetMethod());
        }

        [TestCase]
        public void SetValue_ForPropertyInfoFromRecursiveGetPropertyWithSetter_Works()
        {
            PropertyInfo propInfo = instance.GetType().GetPropertyWithSetterRecursive("IntProperty");

            propInfo.SetValue(instance, 1, null);

            Assert.AreEqual(1, instance.IntProperty);
        }

        [TestCase]
        public void SetPropertyRecursive_Works()
        {
            instance.GetType().SetPropertyRecursive(instance, "IntProperty", 1);

            Assert.AreEqual(1, instance.IntProperty);
        }
    }
}
