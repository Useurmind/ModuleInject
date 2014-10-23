using System.Linq;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    class TestClass
    {
        public string StringProperty { get; set; }
        public TestClass2 Component { get; set; }

        public TestClass()
        {
            
        }

        public TestClass(string value)
        {
            this.StringProperty = value;
        }

        public TestClass(TestClass2 component)
        {
            this.Component = component;
        }

        public void SetStringProperty(string value)
        {
            this.StringProperty = value;
        }

        public void SetComponent(TestClass2 value)
        {
            this.Component = value;
        }
    }
}