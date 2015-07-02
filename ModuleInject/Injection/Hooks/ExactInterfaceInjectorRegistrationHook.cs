using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces.Hooks;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection.Hooks
{
    /// <summary>
    /// A registration hook that applies to all modules and components that implement a given interface.
    /// Also the component must be registered exactly under the given interface.
    /// </summary>
    /// <typeparam name="TIComponent">The interface that the component should implement.</typeparam>
    /// <typeparam name="TIModule">The interface that the module should implement.</typeparam>
    public class ExactInterfaceInjectorRegistrationHook<TIModule, TIComponent> : IRegistrationHook
        where TIModule : class
        where TIComponent : class
    {
        private IExactInterfaceInjector<TIModule, TIComponent> injector;

        /// <param name="injector">The injector that should be applied to all matching registrations.</param>
        public ExactInterfaceInjectorRegistrationHook(IExactInterfaceInjector<TIModule, TIComponent> injector)
        {
            this.injector = injector;
        }
        /// <inheritdoc />
		public bool AppliesToModule(IModule module)
		{
			return (module as TIModule) != null;
		}

        /// <inheritdoc />
        public bool AppliesToRegistration(IInjectionRegister injectionRegister)
		{
            return injectionRegister.ComponentInterface == typeof(TIComponent);
		}

        /// <inheritdoc />
        public void Execute(IInjectionRegister injectionRegister)
		{
			var injectionContext = new ExactInterfaceModificationContext<TIModule, TIComponent>(injectionRegister);

			injector.InjectInto(injectionContext);
		}
    }
}
