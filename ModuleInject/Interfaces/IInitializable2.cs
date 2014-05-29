using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IInitializable<TArgument1, TArgument2>
    {
        void Initialize(TArgument1 dependency1, TArgument2 dependency2);
    }
}
