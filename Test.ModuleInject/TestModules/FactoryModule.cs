using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IFactoryModule : IInjectionModule
    {
        IMainComponent1 CreateComponent1();
        IMainComponent2 CreateComponent2();
    }

    public class FactoryModule : InjectionModule<IFactoryModule, FactoryModule>, IFactoryModule
    {
        [PrivateComponent]
        private IMainComponent1 Component1 { get; set; }

        public FactoryModule()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component1)
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent2)
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent22);

            RegisterPublicComponentFactory<IMainComponent1, MainComponent1>(x => x.CreateComponent1());
            RegisterPublicComponentFactory<IMainComponent2, MainComponent2>(x => x.CreateComponent2());
        }

        public IMainComponent1 CreateComponent1()
        {
            return CreateInstance(x => x.CreateComponent1());
        }

        public IMainComponent2 CreateComponent2()
        {
            return CreateInstance(x => x.CreateComponent2());
        }

        public IMainComponent1 RetrievePrivateComponent1()
        {
            return Component1;
        }
    }
}
