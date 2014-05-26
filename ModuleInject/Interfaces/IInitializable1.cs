using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IInitializable<T1>
    {
        void Initialize(T1 dependency1);
    }
}
