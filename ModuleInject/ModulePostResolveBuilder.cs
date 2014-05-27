using Microsoft.Practices.Unity;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject
{
    public class ModulePostResolveBuilder
    {
        public void PerformPostResolveAssembly(IInjectionModule module, DoubleKeyDictionary<Type, string, IInstanceRegistrationContext> instanceRegistrations)
        {
            foreach (var instanceRegistration in instanceRegistrations)
            {
                PerformPostResolveAssembly(module, instanceRegistration);
            }
        }

        private void PerformPostResolveAssembly(IInjectionModule module, DoubleKeyDictionaryItem<Type, string, IInstanceRegistrationContext> instanceRegistration)
        {
            Type instanceType = instanceRegistration.Key1;
            string instanceName = instanceRegistration.Key2;
            object component = module.GetComponent(instanceType, instanceName);
            if (component == null)
            {
                throw new ModuleInjectException(string.Format("No Component of Type '{0}' with Name '{1}' found for post resolve assembly", instanceType.FullName, instanceName));
            }

            var assemblers = instanceRegistration.Value.PostResolveAssemblers;

            foreach (var assembler in assemblers)
            {
                assembler.Assemble(component, module);
            }
        }
    }
}
