using ModuleInject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

using ModuleInject.Modules;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;

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

    public interface INoAutoWiringModule : IModule
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
            RegisterPrivateComponent(x => x.PrivateAutoWiringComponent).Construct<AutoWiringClass>();
            RegisterPrivateComponent(x => x.PrivateComponent).Construct<MainComponent2>();
        }
    }
}
