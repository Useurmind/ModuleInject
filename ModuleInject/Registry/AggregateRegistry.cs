using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Registry
{
    public class AggregateRegistry : IRegistry
    {
        private IList<IRegistry> aggregatedRegistries;

        public AggregateRegistry()
        {
            aggregatedRegistries= new List<IRegistry>();
        }

        internal void AddRegistry(IRegistry registry)
        {
            aggregatedRegistries.Add(registry);
        }

        internal override bool IsRegistered(Type type)
        {
            bool result = false;
            foreach (var registry in aggregatedRegistries)
            {
                if (registry.IsRegistered(type))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        internal override object GetComponent(Type type)
        {
            object result = null;
            foreach (var registry in aggregatedRegistries)
            {
                if (registry.IsRegistered(type))
                {
                    result = registry.GetComponent(type);
                    break;
                }
            }
            return result;
        }
    }
}
