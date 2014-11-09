using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IRegistrationContext
    {
        IRegistrationTypes RegistrationTypes { get; }
        string RegistrationName { get; }
    }

    public interface IRegistrationContextT
    {
        IRegistrationContext ReflectionContext { get; }
    }
}
