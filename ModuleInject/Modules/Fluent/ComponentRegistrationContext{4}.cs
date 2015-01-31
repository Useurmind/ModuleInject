using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal class ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> : RegistrationContextBase, IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
        where TModule : IModule
        where TComponent : IComponent
        where IModule : Interfaces.IModule
    {
        internal ComponentRegistrationContext(IRegistrationContext context) : base(context)
        {
        }
    }
}
