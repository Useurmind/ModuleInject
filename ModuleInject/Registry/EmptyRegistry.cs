using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Registry
{
    public class EmptyRegistry : IRegistry
    {
        internal override bool IsRegistered(Type type)
        {
            return false;
        }

        internal override object GetComponent(Type type)
        {
            return null;
        }
    }
}
