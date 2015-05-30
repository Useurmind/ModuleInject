using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Injection;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IConstructorInjectionModule : IModule
    {
        // don't change the order, important for test
        IMainComponent1 MainComponent1 { get; }
        IMainComponent2 MainComponent3 { get; }
    }

    public class ConstructorInjectionModule : InjectionModule<ConstructorInjectionModule>, IConstructorInjectionModule
    {
		private int testType = 0;

		// don't change the order, important for test
		public IMainComponent1 MainComponent1
		{
			get
			{
				switch (testType)
				{
					case 1:
						return GetSingleInstance<MainComponent1>();
					case 2:
						return GetSingleInstance(m => new MainComponent1(m.MainComponent2));
					case 3:
						return GetSingleInstance(m => new MainComponent1(m.MainComponent3));
					default:
						return null;
				}
			} }
        public IMainComponent2 MainComponent3 { get { return GetSingleInstance<MainComponent2>(); } }

        [PrivateComponent]
        public IMainComponent2 MainComponent2 { get { return GetSingleInstance<MainComponent2>(); } }

        public void RegisterWithDefaultConstructor()
        {
			testType = 1;
        }

        public void RegisterWithArgumentsInConstructor()
		{
			testType = 2;
        }

        public void RegisterWithArgumentsInConstructorAndArgumentResolvedAfterThis()
		{
			testType = 3;
        }
    }
}
