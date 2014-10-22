using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Lifetime
{
    using ModuleInject.Container.Interface;

    public class DynamicLifetime : ILifetime
    {
        public bool OnObjectResolving()
        {
            return true;
        }

        public void OnObjectResolved(object instance)
        {
        }

        public object OnObjectNotResolved()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
