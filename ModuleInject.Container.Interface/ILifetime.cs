using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Interface
{

    public interface ILifetime : IDisposable
    {
        bool OnObjectResolving();

        void OnObjectResolved(object instance);

        object OnObjectNotResolved();
    }
}
