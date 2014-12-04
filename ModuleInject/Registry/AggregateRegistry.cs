﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Registry
{
    using ModuleInject.Interfaces;

    public class AggregateRegistry : RegistryBase
    {
        private IList<IRegistry> aggregatedRegistries;

        public AggregateRegistry()
        {
            aggregatedRegistries = new List<IRegistry>();
        }

        internal void AddRegistry(IRegistry registry)
        {
            aggregatedRegistries.Add(registry);
        }

        public override bool IsRegistered(Type type)
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

        public override object GetComponent(Type type)
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
