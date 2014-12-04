using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Registry
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
