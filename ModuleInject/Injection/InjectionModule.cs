using ModuleInject.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces;
using ModuleInject.Injection.Hooks;
using ModuleInject.Interfaces.Injection;

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
			return SourceOf<TIComponent>(new FactoryInstantiationStrategy<TIComponent>());
		}

		protected ConstructionContext<TModule, TIComponent> SingleInstance<TIComponent>()
		{
			return SourceOf<TIComponent>(new SingleInstanceInstantiationStrategy<TIComponent>());
		}

		protected ConstructionContext<TModule, TIComponent> SourceOf<TIComponent>(IInstantiationStrategy<TIComponent> instantiationStrategy)
		{
			return new ConstructionContext<TModule, TIComponent>((TModule)this, instantiationStrategy);
		}

		protected override void OnRegistryResolved(IRegistry usedRegistry)
		{
			TryAddRegistrationHooks(usedRegistry);
        }

		protected virtual void OnComponentResolved(ObjectResolvedContext context)
		{
		}

		public void RegisterInjectionRegister(IInjectionRegister injectionRegister)
		{
			injectionRegister.OnResolve(context => this.OnComponentResolved(context));
			this.injectionRegisters.Add(injectionRegister);
		}

		public void TryAddRegistrationHooks(IRegistry usedRegistry)
		{
			var registrationHooksFromRegistry = usedRegistry.GetRegistrationHooks().Where(h => h.AppliesToModule(this));
			var registrationHooksFromModule = this.RegistrationHooks;
			var allRegistrationHooks = registrationHooksFromModule.Union(registrationHooksFromRegistry);
			if (!allRegistrationHooks.Any())
			{
				return;
			}

			foreach (var injectionRegister in this.injectionRegisters)
			{
				foreach (var registrationHook in allRegistrationHooks)
				{
					if (registrationHook.AppliesToRegistration(injectionRegister))
					{
						registrationHook.Execute(injectionRegister);
					}
				}
			}
		}
	}
}
