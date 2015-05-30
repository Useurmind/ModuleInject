using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces.Fluent;
using ModuleInject.Interfaces.Hooks;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection.Hooks
{
    /// <summary>
    /// A registration hook that applies to all modules and components that implement a given interface.
    /// </summary>
    /// <typeparam name="TIComponent">The interface that the component should implement.</typeparam>
    /// <typeparam name="TIModule">The interface that the module should implement.</typeparam>
    public class InterfaceInjectorRegistrationHook<TIModule, TIComponent> : IRegistrationHook
        where TIModule : class
        where TIComponent : class
    {
        private IInterfaceInjector<TIModule, TIComponent> injector;

        /// <param name="injector">The injector that should be applied to all matching registrations.</param>
        public InterfaceInjectorRegistrationHook(IInterfaceInjector<TIModule, TIComponent> injector)
        {
            this.injector = injector;
        }

		public bool AppliesToModule(IModule module)
		{
			return (module as TIModule) != null;
		}
		
		public bool AppliesToRegistration(IInjectionRegister injectionRegister)
		{
			return injectionRegister.ComponentType.GetInterfaces().Contains(typeof(TIComponent));
		}

		public void Execute(IInjectionRegister injectionRegister)
		{
			var interfaceInjectionRegister = new InterfaceInjectionRegister<TIModule, TIComponent>(injectionRegister);

			injector.InjectInto(interfaceInjectionRegister);
		}
    }
}
