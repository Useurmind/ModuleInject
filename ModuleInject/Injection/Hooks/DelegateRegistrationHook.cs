using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Interfaces.Fluent;
using ModuleInject.Interfaces.Hooks;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection.Hooks
{
    /// <summary>
    /// A registration hook that uses actions to perform its duty.
    /// </summary>
    public class DelegateRegistrationHook : IRegistrationHook
    {
        private Func<IModule, bool> appliesToModule;
        private Func<IInjectionRegister, bool> appliesToRegistration;
        private Action<IInjectionRegister> execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateRegistrationHook"/> class.
        /// </summary>
        /// <param name="appliesToModule">The func that is executed for <see cref="AppliesToModule"/>.</param>
        /// <param name="appliesToRegistration">The func that is executed for <see cref="AppliesToRegistration"/>.</param>
        /// <param name="execute">The action that is executed for <see cref="Execute"/>.</param>
        public DelegateRegistrationHook(Func<IModule, bool> appliesToModule, Func<IInjectionRegister, bool> appliesToRegistration, Action<IInjectionRegister> execute)
        {
            this.appliesToModule = appliesToModule;
            this.appliesToRegistration = appliesToRegistration;
            this.execute = execute;
        }

        public bool AppliesToModule(IModule module)
        {
            return this.appliesToModule(module);
        }

		public bool AppliesToRegistration(IInjectionRegister injectionRegister)
		{
			return this.appliesToRegistration(injectionRegister);
		}

		public void Execute(IInjectionRegister injectionRegister)
		{
			this.execute(injectionRegister);
		}
    }
}
