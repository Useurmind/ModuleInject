using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Lifetime
{
    using ModuleInject.Common.Disposing;
    using ModuleInject.Container.Interface;

    public class DynamicLifetime : DisposableExtBase, ILifetime
    {
        public virtual bool OnObjectResolving()
        {
            return true;
        }

        public virtual void OnObjectResolved(ObjectResolvedContext context)
        {
        }

        public virtual object OnObjectNotResolved()
        {
            throw new NotImplementedException();
        }
    }
}
