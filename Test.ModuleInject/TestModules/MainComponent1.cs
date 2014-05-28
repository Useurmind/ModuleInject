using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IMainComponent1
    {
        int InjectedValue { get;  }
        IMainComponent2 MainComponent2 { get;  }
        IMainComponent2 MainComponent22 { get; }
        IMainComponent2 MainComponent23 { get; }
        ISubComponent1 SubComponent1 { get;  }
    }

    public class MainComponent1 : IMainComponent1, 
        IInitializable<IMainComponent2>,
        IInitializable<ISubComponent1>,
        IInitializable<IMainComponent2, ISubComponent1>,
        IInitializable<IMainComponent2, IMainComponent2, ISubComponent1>
    {
        public int InjectedValue { get; set; }
        public IMainComponent2 MainComponent2 { get; set; }
        public IMainComponent2 MainComponent22 { get; set; }
        public IMainComponent2 MainComponent23 { get; set; }
        public ISubComponent1 SubComponent1 { get; set; }

        public void Initialize(IMainComponent2 dependency1)
        {
            MainComponent2 = dependency1;
        }

        public void Initialize(ISubComponent1 dependency1)
        {
            SubComponent1 = dependency1;
        }

        public void Initialize(IMainComponent2 dependency1, ISubComponent1 dependency2)
        {
            MainComponent2 = dependency1;
            SubComponent1 = dependency2;
        }

        public void Initialize(IMainComponent2 dependency1, IMainComponent2 dependency2, ISubComponent1 dependency3)
        {
            MainComponent2 = dependency1;
            MainComponent22 = dependency2;
            SubComponent1 = dependency3;
        }
    }
}
