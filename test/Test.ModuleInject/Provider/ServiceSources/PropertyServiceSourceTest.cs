using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Provider.ServiceSources;
using NUnit.Framework;

namespace Test.ModuleInject.Provider.ServiceSources
{
    [TestFixture]
    public class PropertyServiceSourceTest
    {
        [Test]
        public void Get_ReturnsProperty()
        {
            var instance = "hallo service source";
            var propertyInfo = instance.GetType().GetProperty("Length");

            var source = new PropertyServiceSource(instance, propertyInfo);

            Assert.AreEqual(instance.Length, (int)source.Get());
        }
    }
}
