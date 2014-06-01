using ModuleInject;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Test.ModuleInject.TestModules
{
    public interface IAutoWiringClass
    {
        MainComponent2 PropertyComponent { get; }
        MainComponent2 ConstructorComponent { get; }
    }

    public class AutoWiringClass : IAutoWiringClass
    {
        [Dependency]
        public MainComponent2 PropertyComponent { get; set; }
        public MainComponent2 ConstructorComponent { get; set; }

        public AutoWiringClass()
        {

        }

        public AutoWiringClass(MainComponent2 component)
        {
            ConstructorComponent = component;
        }
    }

    public interface INoAutoWiringModule : IInjectionModule
    {

    }

    public class NoAutoWiringModule : InjectionModule<INoAutoWiringModule, NoAutoWiringModule>, INoAutoWiringModule
    {
        [PrivateComponent]
        public IAutoWiringClass PrivateAutoWiringComponent { get; set; }

        [PrivateComponent]
        public IMainComponent2 PrivateComponent { get; set; }

        public NoAutoWiringModule()
        {
            RegisterPrivateComponent<IAutoWiringClass, AutoWiringClass>(x => x.PrivateAutoWiringComponent);
            RegisterPrivateComponent<IMainComponent2, MainComponent2>(x => x.PrivateComponent);
        }
    }
}
