using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IInitializable<T1, T2>
    {
        void Initialize(T1 dependency1, T2 dependency2);
    }
}
