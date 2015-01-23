using System.Linq;

using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal abstract class RegistrationContextBase : IRegistrationContextT
    {
        internal RegistrationContext Context { get; private set; }

        public IRegistrationContext ReflectionContext
        {
            get
            {
                return this.Context;
            }
        }

        protected RegistrationContextBase(RegistrationContext context)
        {
            this.Context = context;
        }
    }
}
