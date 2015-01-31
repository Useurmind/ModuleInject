using System.Linq;

using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal class InterfaceRegistrationContext<IComponentBase, IModuleBase> : RegistrationContextBase, IInterfaceRegistrationContext<IComponentBase, IModuleBase>
    {
        internal InterfaceRegistrationContext(IRegistrationContext context) : base(context)
        {
        }
    }
}
