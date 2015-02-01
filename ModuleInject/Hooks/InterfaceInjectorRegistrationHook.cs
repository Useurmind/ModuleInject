using ModuleInject.Interfaces;
using ModuleInject.Modules.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces.Fluent;
using ModuleInject.Interfaces.Hooks;

namespace ModuleInject.Hooks
{
    /// <summary>
    /// A registration hook that applies to all modules and components that implement a given interface.
    /// </summary>
    /// <typeparam name="IComponent">The interface that the component should implement.</typeparam>
    /// <typeparam name="IModule">The interface that the module should implement.</typeparam>
    public class InterfaceInjectorRegistrationHook<IComponent, IModule> : IRegistrationHook
        where IModule : class
        where IComponent : class
    {
        private IInterfaceInjector<IComponent, IModule> injector;

        /// <param name="injector">The injector that should be applied to all matching registrations.</param>
        public InterfaceInjectorRegistrationHook(IInterfaceInjector<IComponent, IModule> injector)
        {
            this.injector = injector;
        }

        public bool AppliesToModule(object module)
        {
            return (module as IModule) != null;
        }

        public bool AppliesToRegistration(IRegistrationContext registrationContext)
        {
            return registrationContext.RegistrationTypes.TComponent.GetInterfaces().Contains(typeof(IComponent));
        }

        public void Execute(IRegistrationContext registrationContext)
        {
            var interfaceRegistrationContext = new InterfaceRegistrationContext<IComponent, IModule>(registrationContext);

            injector.InjectInto(interfaceRegistrationContext);
        }
    }
}
