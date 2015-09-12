using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Provider.ServiceSources;
using Xunit;

namespace Test.ModuleInject.Provider.ServiceSources
{
    
    public class PropertyServiceSourceTest
    {
        [Fact]
        public void Get_ReturnsProperty()
        {
            var instance = "hallo service source";
            var propertyInfo = instance.GetType().GetProperty("Length");

            var source = new PropertyServiceSource(instance, propertyInfo);

            Assert.Equal(instance.Length, (int)source.Get());
        }
    }
}
