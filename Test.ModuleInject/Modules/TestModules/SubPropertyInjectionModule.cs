using System.Linq;
using ModuleInject.Injection;
using ModuleInject.Interfaces;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface ISubPropertyInjectionModule : IModule
    {
        ISubComponent2 SubComponent2 { get; }
        ISubComponent1 SubComponent1 { get; }
        IMainComponent2 MainComponent2 { get; }
    }

    public class SubPropertyInjectionModule : InjectionModule<SubPropertyInjectionModule>, ISubPropertyInjectionModule
    {
        public ISubComponent2 SubComponent2 { get { return GetSingleInstance<SubComponent2>(); } }
        public ISubComponent1 SubComponent1
        {
            get
            {
                return GetSingleInstance<ISubComponent1>(cc =>
                {
                cc.Construct<SubComponent1>()
                .Inject((m, c) => c.SubComponent2 = m.MainComponent2.SubComponent2);
                });
            }
        }
        public IMainComponent2 MainComponent2
        {
            get
            {
                return GetSingleInstanceWithConstruct(m => new MainComponent2()
                {
                    SubComponent2 = m.SubComponent2
                });
            }
        }
    }
}
