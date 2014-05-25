using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IMainComponent2
    {
        int IntProperty { get; set; }
    }

    public class MainComponent2 : IMainComponent2
    {
        public int IntProperty { get; set; }
    }
}
