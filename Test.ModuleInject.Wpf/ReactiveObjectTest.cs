using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Wpf;
using NUnit.Framework;

namespace Test.ModuleInject.Wpf
{
    [TestFixture]
    public class ReactiveObjectTest
    {
        private class TestViewModel : ReactiveObject
        {
            private int intProperty;

            public int IntProperty
            {
                get { return intProperty; }
                set { this.SetProperty(ref intProperty, value); }
            }
        }

        [Test]
        public void ObserveIntProperty_SetIntPropertyTwice_ObserverTwoMessages()
        {

        }
    }
}
