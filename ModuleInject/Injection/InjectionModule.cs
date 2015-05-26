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
using System.Runtime.CompilerServices;
using ModuleInject.Interfaces.Hooks;

namespace ModuleInject.Injection
{
	public interface IInjectionModule
	{
		void RegisterInjectionRegister(IInjectionRegister injectionRegister, string componentName = null);
	}

	public class InjectionModule<TModule> : Module, IInjectionModule
		where TModule : InjectionModule<TModule>
	{
		private ModuleMemberExpressionChecker<TModule, TModule> expressionChecker;

		private ISet<IInjectionRegister> injectionRegisters;
		private IDictionary<string, IInjectionRegister> namedInjectionRegisters;

		private IRegistry usedRegistry;
		private IEnumerable<IRegistrationHook> allRegistrationHooks;

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

		protected ConstructionContext<TModule, TIComponent> Factory2<TIComponent>([CallerMemberName]string componentName = null)
		{
			return SourceOf<TIComponent>(componentName, new FactoryInstantiationStrategy<TIComponent>());
		}

		protected ConstructionContext<TModule, TIComponent> SingleInstance2<TIComponent>([CallerMemberName]string componentName = null)
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
			CommonFunctions.CheckNullArgument("instantiationStrategy", instantiationStrategy);

			return new ConstructionContext<TModule, TIComponent>((TModule)this, instantiationStrategy, componentName);
		}

		protected ConstructionContext<TModule, TIComponent> SourceOf<TIComponent>(IInstantiationStrategy<TIComponent> instantiationStrategy)
		{
			return new ConstructionContext<TModule, TIComponent>((TModule)this, instantiationStrategy);
		}

		protected override void OnRegistryResolved(IRegistry usedRegistry)
		{
			this.usedRegistry = usedRegistry;

			var registrationHooksFromRegistry = this.usedRegistry.GetRegistrationHooks().Where(h => h.AppliesToModule(this));
			var registrationHooksFromModule = this.RegistrationHooks;
			allRegistrationHooks = registrationHooksFromModule.Union(registrationHooksFromRegistry);

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
				if (this.IsResolved)
				{
					this.TryAddRegistrationHooks(injectionRegister);
					this.TryAddModuleResolveHook(injectionRegister);
				}

				this.namedInjectionRegisters.Add(componentName, injectionRegister);
			}
		}

		private void TryAddRegistrationHooks()
		{
			foreach (var injectionRegister in this.injectionRegisters)
			{
				TryAddRegistrationHooks(injectionRegister);
            }
		}

		private void TryAddRegistrationHooks(IInjectionRegister injectionRegister)
		{
			if (!allRegistrationHooks.Any())
			{
				return;
			}

			foreach (var registrationHook in allRegistrationHooks)
			{
				if (registrationHook.AppliesToRegistration(injectionRegister))
				{
					registrationHook.Execute(injectionRegister);
				}
			}
		}

		protected TIComponent Get<TIComponent>(Expression<Func<TModule, TIComponent>> componentMember)
		{
			CommonFunctions.CheckNullArgument("componentMember", componentMember);

			var componentName = GetComponentName(componentMember);

			return (TIComponent)Get(componentName.Name);
		}

		protected TComponent GetFactory<TComponent>(
			[CallerMemberName]string componentName = null)
			where TComponent : new()
		{
			return GetFactory(m => new TComponent(), componentName);
		}

		protected TComponent GetFactory<TComponent>(
			Func<TModule, TComponent> construct,
			[CallerMemberName]string componentName = null)
		{
			Action<ConstructionContext<TModule, TComponent>> registerComponent = cc =>
			{
				cc.Construct(construct);
			};
			return GetFactory(registerComponent, componentName);
		}

		protected TIComponent GetFactory<TIComponent>(
			Action<ConstructionContext<TModule, TIComponent>> registerComponent,
            [CallerMemberName]string componentName = null)
		{
			return Get<TIComponent>(registerComponent, new FactoryInstantiationStrategy<TIComponent>(), componentName);
		}

		protected TComponent GetSingleInstance<TComponent>(
			[CallerMemberName]string componentName = null)
			where TComponent : new()
		{
			return GetSingleInstance(m => new TComponent(), componentName);
		}

		protected TComponent GetSingleInstance<TComponent>(
			Func<TModule, TComponent> construct,
			[CallerMemberName]string componentName = null)
		{
			Action<ConstructionContext<TModule, TComponent>> registerComponent = cc =>
			{
				cc.Construct(construct);
			};
			return GetSingleInstance(registerComponent, componentName);
		}

		protected TIComponent GetSingleInstance<TIComponent>(
			Action<ConstructionContext<TModule, TIComponent>> registerComponent,
            [CallerMemberName]string componentName = null)
		{
			return Get<TIComponent>(registerComponent, new SingleInstanceInstantiationStrategy<TIComponent>(), componentName);
		}

		protected TIComponent Get<TIComponent>(
			Action<ConstructionContext<TModule, TIComponent>> registerComponent,
			IInstantiationStrategy<TIComponent> instantiationStrategy,
			[CallerMemberName]string componentName = null)
		{
			if (!this.namedInjectionRegisters.ContainsKey(componentName))
			{
				var constructionContext = this.SourceOf<TIComponent>(componentName, instantiationStrategy);
				registerComponent(constructionContext);
			}

			return (TIComponent)Get(componentName);
		}

		protected TIComponent Get<TIComponent>([CallerMemberName]string componentName = null)
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
			foreach (var injectionRegister in injectionRegisters)
			{
				TryAddModuleResolveHook(injectionRegister);
			}
		}

		void TryAddModuleResolveHook(IInjectionRegister injectionRegister)
		{
			var moduleTypeName = typeof(IModule).Name;
			var isModule = injectionRegister.ComponentType.GetInterface(moduleTypeName) != null;
			if (isModule)
			{
				injectionRegister.OnResolve(this.OnRegisteredModuleResolved);
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
