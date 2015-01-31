using System.Linq;

using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal abstract class RegistrationContextBase : IOuterRegistrationContext
    {
        public IRegistrationContext Context { get; private set; }
        
        protected RegistrationContextBase(IRegistrationContext context)
        {
            this.Context = context;
        }
    }
}
