using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface ISubComponent1 {
        ISubComponent2 SubComponent2 { get;set;}
    }

    public class SubComponent1 : ISubComponent1
    {
        public ISubComponent2 SubComponent2 { get; set; }
    }
}
