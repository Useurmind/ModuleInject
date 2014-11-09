using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IRegistrationTypes
    {
        Type IComponent { get; }
        Type TComponent { get; }
        Type IModule { get; }
        Type TModule { get; }
    }
}
