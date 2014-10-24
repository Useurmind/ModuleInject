using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Interface
{
    public class ObjectResolvedContext
    {
        public Type RegisteredType { get; set; }
        public Type ActualType { get; set; }
        public string Name { get; set; }
        public Object Instance { get; set; }
    }

    public interface ILifetime : IDisposable
    {
        bool OnObjectResolving();

        void OnObjectResolved(ObjectResolvedContext context);

        object OnObjectNotResolved();
    }
}
