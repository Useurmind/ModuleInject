using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IMainComponent2 : IMainComponent2SubInterface
    {
        int IntProperty { get; set; }
        IMainComponent2SubInterface Component2Sub { get; set; }
    }

    public interface IMainComponent2SubInterface
    {
        string StringProperty { get; set; }
    }

    public class MainComponent2 : IMainComponent2, IInitializable<IMainComponent2SubInterface>
    {
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
        public IMainComponent2SubInterface Component2Sub { get; set; }

        public void Initialize(IMainComponent2SubInterface dependency1)
        {
            Component2Sub = dependency1;
        }
    }
}
