using System.Linq;

using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal abstract class RegistrationContextBase : IRegistrationContextT
    {
        internal IRegistrationContext Context { get; private set; }

        public IRegistrationContext ReflectionContext
        {
            get
            {
                return this.Context;
            }
        }

        protected RegistrationContextBase(IRegistrationContext context)
        {
            this.Context = context;
        }
    }
}
