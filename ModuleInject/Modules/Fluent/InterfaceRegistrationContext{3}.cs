using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal class InterfaceRegistrationContext<IComponent, IModuleBase, TModule> : RegistrationContextBase, IInterfaceRegistrationContext<IComponent, IModuleBase, TModule>
        where TModule : IModuleBase
        where IModuleBase : IModule
    {
        internal InterfaceRegistrationContext(IRegistrationContext context) : base(context)
        {
        }
    }
}
