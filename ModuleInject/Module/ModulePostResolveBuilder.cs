using Microsoft.Practices.Unity;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ModuleInject.Module
{
    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Utility;

    public static class ModulePostResolveBuilder
    {
        public static void PerformPostResolveAssembly(IInjectionModule module, DoubleKeyDictionary<Type, string, IGatherPostResolveAssemblers> assemblerCollectionDictionary)
        {
            CommonFunctions.CheckNullArgument("module", module);
            CommonFunctions.CheckNullArgument("assemblerCollection", assemblerCollectionDictionary);

            foreach (var instanceRegistration in assemblerCollectionDictionary)
            {
                PerformPostResolveAssembly(module, instanceRegistration);
            }
        }

        private static void PerformPostResolveAssembly(IInjectionModule module, DoubleKeyDictionaryItem<Type, string, IGatherPostResolveAssemblers> assemblerCollection)
        {
            Type objectType = assemblerCollection.Key1;
            string objectName = assemblerCollection.Key2;
            object component = module.GetComponent(objectType, objectName);
            if (component == null)
            {
                throw new ModuleInjectException(string.Format(CultureInfo.CurrentCulture, "No Component of Type '{0}' with Name '{1}' found for post resolve assembly", objectType.FullName, objectName));
            }

            var assemblers = assemblerCollection.Value.PostResolveAssemblers;

            foreach (var assembler in assemblers)
            {
                assembler.Assemble(component, module);
            }
        }
    }
}
