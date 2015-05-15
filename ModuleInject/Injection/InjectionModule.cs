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
		void RegisterInjectionRegister(IInjectionRegister injectionRegister);
    }

	public class InjectionModule<TModule> : Module, IInjectionModule
		where TModule : InjectionModule<TModule>
	{
		private ISet<IInjectionRegister> injectionRegisters;

		public InjectionModule()
		{
			this.injectionRegisters = new HashSet<IInjectionRegister>();
		}

		protected ConstructionContext<TModule, TIComponent> Factory<TIComponent>()
		{
			return new ConstructionContext<TModule, TIComponent>((TModule)this, new FactoryInstantiationStrategy<TIComponent>());
		}

		protected ConstructionContext<TModule, TIComponent> SingleInstance<TIComponent>()
		{
			return new ConstructionContext<TModule, TIComponent>((TModule)this, new SingleInstanceInstantiationStrategy<TIComponent>());
		}

		protected override void OnRegistryResolved(IRegistry usedRegistry)
		{

		}

		public void RegisterInjectionRegister(IInjectionRegister injectionRegister)
		{
			this.injectionRegisters.Add(injectionRegister);
		}
	}
}
