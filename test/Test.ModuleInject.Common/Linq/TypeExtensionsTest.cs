using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using Xunit;

namespace Test.ModuleInject.Common.Linq
{
    using global::ModuleInject.Common.Linq;

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

        public TypeExtensionsTest()
        {
            instance = new DerivedClass();
        }

        [Fact]
        public void Verify_StandardGetPropertyHasNoSetter()
        {
            PropertyInfo propInfo = instance.GetType().GetProperty("IntProperty");

            Assert.False(propInfo.CanWrite);
            Assert.Null(propInfo.GetSetMethod());
        }

        [Fact]
        public void SetValue_ForPropertyInfoFromRecursiveGetPropertyWithSetter_Works()
        {
            PropertyInfo propInfo = instance.GetType().GetPropertyWithSetterRecursive("IntProperty");

            propInfo.SetValue(instance, 1, null);

            Assert.Equal(1, instance.IntProperty);
        }

        [Fact]
        public void SetPropertyRecursive_Works()
        {
            instance.GetType().SetPropertyRecursive(instance, "IntProperty", 1);

            Assert.Equal(1, instance.IntProperty);
        }
    }
}
