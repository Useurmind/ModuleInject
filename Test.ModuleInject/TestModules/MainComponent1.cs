using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IMainComponent1
    {
        IMainComponent2 MainComponent2 { get; set; }
        ISubComponent1 SubComponent1 { get; set; }
    }

    public class MainComponent1 : IMainComponent1
    {
        public IMainComponent2 MainComponent2 { get; set; }
        public ISubComponent1 SubComponent1 { get; set; }
    }
}
