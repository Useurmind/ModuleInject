using ModuleInject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface IInjectorModule : IModule
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
            RegisterPublicComponent(x => x.Component2).Construct<MainComponent2>();
            RegisterPublicComponent(x => x.Component22).Construct<MainComponent2>();
            RegisterPublicComponent(x => x.Component23).Construct<MainComponent2>();
        }

        public void RegisterClassInjectorWithPropertyValueAndInitialize1()
        {
            RegisterPublicComponent(x => x.Component1)
                .Construct<MainComponent1>()
                .AddTypeInjection(component =>
                {
                    component.Inject((c, m) => c.Initialize(m.Component2));
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
            RegisterPublicComponent(x => x.Component1)
                .Construct<MainComponent1>()
                .AddInterfaceInjection(component =>
                {
                    component.Inject((c, m) => c.Initialize(m.Component2));
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
