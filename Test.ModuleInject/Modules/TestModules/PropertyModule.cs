using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Injection;
using ModuleInject.Interfaces;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IPropertyModule : IModule
    {
        IMainComponent1 InstanceRegistrationComponent { get; }
        IMainComponent1 InitWithPropertiesComponent { get; }
        IMainComponent1 InitWithInitialize1Component { get; }
        IMainComponent1 InitWithInitialize1FromSubComponent { get; }
        IMainComponent1 InitWithInitialize2Component { get; }
        IMainComponent1 InitWithInitialize3Component { get; }
        IMainComponent1 InitWithInjectorComponent { get; }
        IMainComponent2 Component2 { get; }
        IMainComponent2 Component22 { get; }

        IMainComponent1 AlsoRegisterForComponent { get; }

        //ISubModule SubModule { get; set; }

        IMainComponent2 CreateComponent2();
    }

    public class PropertyModule : InjectionModule<PropertyModule>, IPropertyModule
    {
        public MainComponent1 FixedInstance { get; private set; }
        public int InjectedValue { get; private set; }

        public IMainComponent1 InstanceRegistrationComponent
        {
            get
            {
                return this.GetSingleInstance<IMainComponent1>(cc =>
                {
                    cc.Construct(m => this.FixedInstance)
                      .Inject((m, c) => c.MainComponent22 = m.Component22)
                      .Inject((m, c) => c.MainComponent23 = m.PrivateComponent)
                      .Inject((m, c) => c.SubComponent1 = m.SubModule.Component1)
                      .Inject((m, c) => c.Initialize(m.Component2));
                });
            }
        }
        public IMainComponent1 InitWithPropertiesComponent
        {
            get
            {
                return this.GetSingleInstance<IMainComponent1>(cc =>
                {
                    cc.Construct<MainComponent1>()
                      .Inject((m, c) => c.MainComponent2 = m.Component2)
                      .Inject((m, c) => c.SubComponent1 = m.SubModule.Component1)
                      .Inject((m, c) => c.MainComponent22 = m.PrivateComponent)
                      .Inject((m, c) => c.MainComponent23 = m.PrivateInstanceComponent)
                      .Inject((m, c) => c.ComponentViaSubinterface = m.Component2)
                      .Inject((m, c) => c.InjectedValue = m.InjectedValue);
                });
            }
        }
        public IMainComponent1 InitWithInitialize1Component
        {
            get
            {
                return this.GetSingleInstance<IMainComponent1>(cc =>
                {
                    cc.Construct<MainComponent1>()
                      .Inject((m, c) => c.Initialize(m.Component2));
                });
            }
        }
        public IMainComponent1 InitWithInitialize1FromSubComponent
        {
            get
            {
                return this.GetSingleInstance<IMainComponent1>(cc =>
                {
                    cc.Construct<MainComponent1>()
                      .Inject((m, c) => c.Initialize(m.SubModule.Component1));
                });
            }
        }
        public IMainComponent1 InitWithInitialize2Component
        {
            get
            {
                return this.GetSingleInstance<IMainComponent1>(cc =>
                {
                    cc.Construct<MainComponent1>()
                      .Inject((m, c) => c.Initialize(m.Component2, m.SubModule.Component1));
                });
            }
        }
        public IMainComponent1 InitWithInitialize3Component
        {
            get
            {
                return this.GetSingleInstance<IMainComponent1>(cc =>
                {
                    cc.Construct<MainComponent1>()
                      .Inject((m, c) => c.Initialize(m.Component2, m.Component22, m.SubModule.Component1));
                });
            }
        }
        public IMainComponent1 InitWithInjectorComponent
        {
            get
            {
                return this.GetSingleInstance<IMainComponent1>(cc =>
                {
                    cc.Construct<MainComponent1>()
                        .AddInjector(new InterfaceInjector<PropertyModule, MainComponent1>(context =>
                        {
                            context.Inject((m, c) => c.InjectedValue = m.InjectedValue);
                        }))
                        .AddInjector(new InterfaceInjector<PropertyModule, MainComponent1>(context =>
                        {
                            context.Inject((m, c) => c.MainComponent2 = m.Component2);
                            context.Inject((m, c) => c.MainComponent22 = m.Component22);
                        }))
                        .AddInjector(new InterfaceInjector<PropertyModule, MainComponent1>(context =>
                        {
                            context.Inject((m, c) => c.SubComponent1 = m.SubModule.Component1);
                        }));
                });
            }
        }
        public IMainComponent2 Component2 { get { return GetSingleInstance<MainComponent2>(); } }
        public IMainComponent2 Component22
        {
            get
            {
                return GetSingleInstance<IMainComponent2>(cc =>
                {
                    cc.Construct<MainComponent2>()
                    .Inject((m, c) => c.Initialize((IMainComponent2SubInterface)m.Component2));
                });
            }
        }

        public IMainComponent1 AlsoRegisterForComponent { get { return Get<IMainComponent1>(m => m.InitWithInitialize1Component); } }


        public IMainComponent1 PrivateComponentInjectedProperties
        {
            get
            {
                return GetSingleInstance<IMainComponent1>(cc =>
                {
                    cc.Construct<MainComponent1>()
                    .Inject((m, c) => c.MainComponent2 = m.PrivateComponent)
                    .Inject((m, c) => c.MainComponent22 = m.PrivateInstanceComponent)
                    .Inject((m, c) => c.MainComponent23 = m.Component2);
                });
            }
        }

        public IMainComponent2 PrivateComponent { get { return GetSingleInstance<MainComponent2>(); } }

        public IMainComponent2 PrivateInstanceComponent { get { return GetSingleInstance(m => new MainComponent2()); } }
        
        [FromRegistry]
        public ISubModule SubModule { get; set; }

        public PropertyModule()
        {
            this.FixedInstance = new MainComponent1();
            this.InjectedValue = 8;
        }

        public IMainComponent2 CreateComponent2()
        {
            return this.GetFactory<MainComponent2>();
        }
    }
}
