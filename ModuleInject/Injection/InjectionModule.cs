using ModuleInject.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces;

namespace ModuleInject.Injection
{
	public interface IInjectionModule
	{
		void RegisterInjectionRegister<TModule, TIComponent, TComponent>(IInjectionRegister<TModule, TIComponent, TComponent> injectionRegister);
    }

	public class InjectionModule<TModule> : Module, IInjectionModule
		where TModule : InjectionModule<TModule>
	{
		private ISet<IInjectionRegister> injectionRegisters;

		public InjectionModule()
		{
			this.injectionRegisters = new HashSet<IInjectionRegister>();
		}

		protected FactoryConstructionContext<TModule, TIComponent> Factory<TIComponent>()
		{
			return new FactoryConstructionContext<TModule, TIComponent>((TModule)this);
		}

		protected SingletonConstructionContext<TModule, TIComponent> Singleton<TIComponent>()
		{
			return new SingletonConstructionContext<TModule, TIComponent>((TModule)this);
		}

		protected override void OnRegistryResolved(IRegistry usedRegistry)
		{

		}

		public void RegisterInjectionRegister<TModule, TIComponent, TComponent>(IInjectionRegister<TModule, TIComponent, TComponent> injectionRegister)
		{
			this.injectionRegisters.Add(injectionRegister);
		}
	}
}
