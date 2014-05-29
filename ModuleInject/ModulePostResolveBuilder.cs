using Microsoft.Practices.Unity;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ModuleInject
{
    public static class ModulePostResolveBuilder
    {
        public static void PerformPostResolveAssembly(IInjectionModule module, DoubleKeyDictionary<Type, string, IInstanceRegistrationContext> instanceRegistrations)
        {
            CommonFunctions.CheckNullArgument("module", module);
            CommonFunctions.CheckNullArgument("instanceRegistrations", instanceRegistrations);

            foreach (var instanceRegistration in instanceRegistrations)
            {
                PerformPostResolveAssembly(module, instanceRegistration);
            }
        }

        private static void PerformPostResolveAssembly(IInjectionModule module, DoubleKeyDictionaryItem<Type, string, IInstanceRegistrationContext> instanceRegistration)
        {
            Type instanceType = instanceRegistration.Key1;
            string instanceName = instanceRegistration.Key2;
            object component = module.GetComponent(instanceType, instanceName);
            if (component == null)
            {
                throw new ModuleInjectException(string.Format(CultureInfo.CurrentCulture, "No Component of Type '{0}' with Name '{1}' found for post resolve assembly", instanceType.FullName, instanceName));
            }

            var assemblers = instanceRegistration.Value.PostResolveAssemblers;

            foreach (var assembler in assemblers)
            {
                assembler.Assemble(component, module);
            }
        }
    }
}
