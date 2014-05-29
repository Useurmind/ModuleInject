using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IInitializable<TArgument1, TArgument2, TArgument3, TArgument4>
    {
        void Initialize(TArgument1 dependency1, TArgument2 dependency2, TArgument3 dependency3, TArgument4 dependency4);
    }
}
