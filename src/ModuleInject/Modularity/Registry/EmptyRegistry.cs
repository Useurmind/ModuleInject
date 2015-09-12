using ModuleInject.Interfaces.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModuleInject.Modularity.Registry
{
    /// <summary>
    /// USed instead of null. Makes life easier.
    /// </summary>
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

        public override IEnumerable<IRegistrationHook> GetRegistrationHooks()
        {
            return new IRegistrationHook[0];
        }
    }
}
