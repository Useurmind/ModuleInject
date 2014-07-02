using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public interface IGatherPostResolveAssemblers
    {
        IList<IPostResolveAssembler> PostResolveAssemblers { get; }
    }
}
