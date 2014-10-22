using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Interface
{
    public interface IDependencyContainer
    {
        object Resolve(string name, Type type);
    }
}
