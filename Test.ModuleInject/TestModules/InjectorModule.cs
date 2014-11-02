using ModuleInject;
using ModuleInject.Fluent;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface IInjectorModule : IInjectionModule
    {
        IMainComponent1 Component1 { get; }

        IMainComponent2 Component2 { get; }
        IMainComponent2 Component22 { get; }
        IMainComponent2 Component23 { get; }
    }

    public class InjectorModule : InjectionModule<IInjectorModule, InjectorModule>, IInjectorModule
    {
        public IMainComponent1 Component1 { get; private set; }

        public IMainComponent2 Component2 { get; set; }
        public IMainComponent2 Component22 { get; set; }
        public IMainComponent2 Component23 { get; set; }

        public InjectorModule()
        {
            RegisterPublicComponent<IMainComponent2, MainComponent2>(x => x.Component2);
            RegisterPublicComponent<IMainComponent2, MainComponent2>(x => x.Component22);
            RegisterPublicComponent<IMainComponent2, MainComponent2>(x => x.Component23);
        }

        public void RegisterClassInjectorWithPropertyValueAndInitialize1()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.Component1)
                .AddTypeInjection(component =>
                {
                    component.InitializeWith(x => x.Component2);
                })
                .AddTypeInjection(component =>
                {
                    component.Inject(x => x.Component22).IntoProperty(x => x.MainComponent22);
                })
                .AddTypeInjection(component =>
                {
                    component.Inject((IMainComponent2)new MainComponent2()).IntoProperty(x => x.MainComponent23);
                });
        }

        public void RegisterInterfaceInjectorWithPropertyValueAndInitialize1()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.Component1)
                .AddInterfaceInjection(component =>
                {
                    component.InitializeWith(x => x.Component2);
                })
                .AddInterfaceInjection(component =>
                {
                    component.Inject(x => x.Component22).IntoProperty(x => x.MainComponent22);
                })
                .AddInterfaceInjection(component =>
                {
                    component.Inject((IMainComponent2)new MainComponent2()).IntoProperty(x => x.MainComponent23);
                });
        }
    }
}
