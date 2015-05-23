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
		void RegisterInjectionRegister(IInjectionRegister injectionRegister, string componentName =null);
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

		protected ConstructionContext<TModule, TIComponent> Factory<TIComponent>(string componentName)
		{
			return SourceOf<TIComponent>(componentName, new FactoryInstantiationStrategy<TIComponent>());
		}

		protected ConstructionContext<TModule, TIComponent> SingleInstance<TIComponent>(string componentName)
		{
			return SourceOf<TIComponent>(componentName, new SingleInstanceInstantiationStrategy<TIComponent>());
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
			
			var componentName = GetComponentName(componentMember);

			return SourceOf<TIComponent>(componentName.Name, instantiationStrategy);
		}

		protected ConstructionContext<TModule, TIComponent> SourceOf<TIComponent>(
			string componentName,
			IInstantiationStrategy<TIComponent> instantiationStrategy)
		{
			return new ConstructionContext<TModule, TIComponent>((TModule)this, instantiationStrategy, componentName);
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

			var componentName = GetComponentName(componentMember);

			return (TIComponent)Get(componentName.Name);
		}

		protected TIComponent Get<TIComponent>(string componentName)
		{
			return (TIComponent)Get(componentName);
		}

		private System.Reflection.MemberInfo GetComponentName<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
		{
			Expression body = componentMember.Body;
			if (body.NodeType == ExpressionType.MemberAccess)
			{
				var member = (MemberExpression)body;
				return member.Member;
			}
			else if (body.NodeType == ExpressionType.Call)
			{
				var method = (MethodCallExpression)body;
				return method.Method;
            }

			throw new ArgumentException("Expression for component not correct.");
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
