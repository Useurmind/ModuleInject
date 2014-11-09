using System.Linq;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces.Fluent;

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
