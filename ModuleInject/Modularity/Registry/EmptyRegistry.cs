using System;
using System.Linq;

namespace ModuleInject.Modularity.Registry
{
    public class EmptyRegistry : RegistryBase
    {
        public override bool IsRegistered(Type type)
        {
            return false;
        }

        public override object GetComponent(Type type)
        {
            return null;
        }
    }
}
