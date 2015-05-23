using ModuleInject.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces;
using ModuleInject.Injection.Hooks;
using ModuleInject.Interfaces.Injection;
using System.Linq.Expressions;
using ModuleInject.Common.Utility;
using ModuleInject.Utility;
using ModuleInject.Common.Exceptions;

namespace ModuleInject.Injection
{
	public interface IInjectionModule
	{
		void RegisterInjectionRegister(IInjectionRegister injectionRegister, string componentName=null);
	}

	public class InjectionModule<TModule> : Module, IInjectionModule
		where TModule : InjectionModule<TModule>
	{
		private ModuleMemberExpressionChecker<TModule, TModule> expressionChecker;

		private ISet<IInjectionRegister> injectionRegisters;
		private IDictionary<string, IInjectionRegister> namedInjectionRegisters;

		private IRegistry usedRegistry;

		public InjectionModule()
		{
			this.expressionChecker = new ModuleMemberExpressionChecker<TModule, TModule>();
            this.injectionRegisters = new HashSet<IInjectionRegister>();
			this.namedInjectionRegisters = new Dictionary<string, IInjectionRegister>();
        }

		protected ConstructionContext<TModule, TIComponent> Factory<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
		{
			return SourceOf<TIComponent>(componentMember, new FactoryInstantiationStrategy<TIComponent>());
		}

		protected ConstructionContext<TModule, TIComponent> SingleInstance<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
		{
			return SourceOf<TIComponent>(componentMember, new SingleInstanceInstantiationStrategy<TIComponent>());
		}

		protected ConstructionContext<TModule, TIComponent> Factory<TIComponent>()
		{
			return SourceOf<TIComponent>(new FactoryInstantiationStrategy<TIComponent>());
		}

		protected ConstructionContext<TModule, TIComponent> SingleInstance<TIComponent>()
		{
			return SourceOf<TIComponent>(new SingleInstanceInstantiationStrategy<TIComponent>());
		}

		protected ConstructionContext<TModule, TIComponent> SourceOf<TIComponent>(
			Expression<Func<TModule, TIComponent>> componentMember,
			IInstantiationStrategy<TIComponent> instantiationStrategy)
		{
			CommonFunctions.CheckNullArgument("componentMember", componentMember);

			string memberName = expressionChecker.CheckExpressionDescribesDirectMemberAndGetMemberName(componentMember);

			return new ConstructionContext<TModule, TIComponent>((TModule)this, instantiationStrategy, memberName);
		}

		protected ConstructionContext<TModule, TIComponent> SourceOf<TIComponent>(IInstantiationStrategy<TIComponent> instantiationStrategy)
		{
			return new ConstructionContext<TModule, TIComponent>((TModule)this, instantiationStrategy);
		}

		protected override void OnRegistryResolved(IRegistry usedRegistry)
		{
			this.usedRegistry = usedRegistry;
			TryAddRegistrationHooks();
			TryAddModuleResolveHooks();
		}

		protected virtual void OnComponentResolved(ObjectResolvedContext context)
		{
		}

		public void RegisterInjectionRegister(IInjectionRegister injectionRegister, string componentName = null)
		{
			injectionRegister.OnResolve(context => this.OnComponentResolved(context));
			this.injectionRegisters.Add(injectionRegister);
			if (!string.IsNullOrEmpty(componentName))
			{
				this.namedInjectionRegisters.Add(componentName, injectionRegister);
			}
		}

		public void TryAddRegistrationHooks()
		{
			var registrationHooksFromRegistry = this.usedRegistry.GetRegistrationHooks().Where(h => h.AppliesToModule(this));
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

		protected TIComponent Get<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
		{
			CommonFunctions.CheckNullArgument("componentMember", componentMember);

			string componentName = expressionChecker.CheckExpressionDescribesDirectMemberAndGetMemberName(componentMember);

			return (TIComponent)Get(componentName);
		}

		protected object Get(string componentName)
		{
			return this.namedInjectionRegisters[componentName].GetInstance();
		}

		private void TryAddModuleResolveHooks()
		{
			var moduleTypeName = typeof(IModule).Name;
			var moduleInjectionRegisters = this.injectionRegisters.Where(reg => reg.ComponentType.GetInterface(moduleTypeName) != null);
			foreach (var registeredModule in moduleInjectionRegisters)
			{
				registeredModule.OnResolve(this.OnRegisteredModuleResolved);
			}
		}

		private void OnRegisteredModuleResolved(ObjectResolvedContext context)
		{
			var module = context.Instance as IModule;
			if (module != null)
			{
				module.Resolve(this.usedRegistry);
			}
		}
	}
}
