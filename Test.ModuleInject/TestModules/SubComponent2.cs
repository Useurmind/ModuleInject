using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface ISubComponent2
    {
        int IntProperty { get; set; }
    }

    public class SubComponent2 : ISubComponent2
    {
        public int IntProperty { get; set; }
    }
}
