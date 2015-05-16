using System;
using System.Collections.Generic;
using System.Linq;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Hooks;

namespace ModuleInject.Modularity.Registry
{
    public class AggregateRegistry : RegistryBase
    {
        private IList<IRegistry> aggregatedRegistries;
        private IEnumerable<IRegistrationHook> registrationHooks;

        public AggregateRegistry()
        {
            this.aggregatedRegistries = new List<IRegistry>();
        }

        internal void AddRegistry(IRegistry registry)
        {
            this.aggregatedRegistries.Add(registry);
            registrationHooks = null;
        }

        public override bool IsRegistered(Type type)
        {
            bool result = false;
            foreach (var registry in this.aggregatedRegistries)
            {
                if (registry.IsRegistered(type))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public override object GetComponent(Type type)
        {
            object result = null;
            foreach (var registry in this.aggregatedRegistries)
            {
                if (registry.IsRegistered(type))
                {
                    result = registry.GetComponent(type);
                    break;
                }
            }
            return result;
        }

        public override IEnumerable<IRegistrationHook> GetRegistrationHooks()
        {
            if(this.registrationHooks == null)
            {
                this.registrationHooks = this.aggregatedRegistries.SelectMany(x => x.GetRegistrationHooks());
            }

            return this.registrationHooks;
        }
    }
}
