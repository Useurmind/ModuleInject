using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Interfaces.Fluent;
using ModuleInject.Interfaces.Hooks;

namespace ModuleInject.Hooks
{
    /// <summary>
    /// A registration hook that uses actions to perform its duty.
    /// </summary>
    public class DelegateRegistrationHook : IRegistrationHook
    {
        private Func<object, bool> appliesToModule;
        private Func<IRegistrationContext, bool> appliesToRegistration;
        private Action<IRegistrationContext> execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateRegistrationHook"/> class.
        /// </summary>
        /// <param name="appliesToModule">The func that is executed for <see cref="AppliesToModule"/>.</param>
        /// <param name="appliesToRegistration">The func that is executed for <see cref="AppliesToRegistration"/>.</param>
        /// <param name="execute">The action that is executed for <see cref="Execute"/>.</param>
        public DelegateRegistrationHook(Func<object, bool> appliesToModule, Func<IRegistrationContext, bool> appliesToRegistration, Action<IRegistrationContext> execute)
        {
            this.appliesToModule = appliesToModule;
            this.appliesToRegistration = appliesToRegistration;
            this.execute = execute;
        }

        public bool AppliesToModule(object module)
        {
            return this.appliesToModule(module);
        }

        public bool AppliesToRegistration(IRegistrationContext registrationContext)
        {
            return this.appliesToRegistration(registrationContext);
        }

        public void Execute(IRegistrationContext registrationContext)
        {
            this.execute(registrationContext);
        }
    }
}
