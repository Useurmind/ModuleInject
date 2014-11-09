using System.Linq;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public class InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> : IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        internal RegistrationContext Context { get; private set; }

        public IRegistrationContext ReflectionContext
        {
            get
            {
                return Context;
            }
        }

        internal InstanceRegistrationContext(RegistrationContext context)
        {
            this.Context = context;
        }
    }
}