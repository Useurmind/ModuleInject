using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public interface IInstanceRegistrationContext
    {
        IList<IPostResolveAssembler> PostResolveAssemblers { get; }
    }
}
