using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IInitializable<TArgument>
    {
        void Initialize(TArgument dependency1);
    }
}
