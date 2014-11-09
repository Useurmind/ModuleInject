using System.Linq;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;
    using ModuleInject.Module;

    internal class InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> : RegistrationContextBase, IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        internal InstanceRegistrationContext(RegistrationContext context) : base(context)
        {
        }
    }
}