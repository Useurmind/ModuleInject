using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Hooks
{
    /// <summary>
    /// A registration hook that uses actions to perform its duty.
    /// </summary>
    public class DelegateRegistrationHook : IRegistrationHook
    {
        private Func<object, bool> appliesToModule;
        private Func<IRegistrationContext, bool> appliesToComponent;
        private Action<IRegistrationContext> execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateRegistrationHook"/> class.
        /// </summary>
        /// <param name="appliesToModule">The func that is executed for <see cref="AppliesToModule"/>.</param>
        /// <param name="appliesToComponent">The func that is executed for <see cref="AppliesToComponent"/>.</param>
        /// <param name="execute">The action that is executed for <see cref="Execute"/>.</param>
        public DelegateRegistrationHook(Func<object, bool> appliesToModule, Func<IRegistrationContext, bool> appliesToComponent, Action<IRegistrationContext> execute)
        {
            this.appliesToModule = appliesToModule;
            this.appliesToComponent = appliesToComponent;
            this.execute = execute;
        }

        public bool AppliesToModule(object module)
        {
            throw new NotImplementedException();
        }

        public bool AppliesToComponent(Interfaces.Fluent.IRegistrationContext registrationContext)
        {
            throw new NotImplementedException();
        }

        public void Execute(Interfaces.Fluent.IRegistrationContext registrationContext)
        {
            throw new NotImplementedException();
        }
    }
}
