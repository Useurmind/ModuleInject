using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Injection;
using ModuleInject.Interfaces;

namespace ModuleInject.Provider.ProviderFactory
{
    public static class ModuleServiceProviderExtensions
    {
        public static void FromModule<TModule>(this ServiceProvider serviceProvider, TModule module)
            where TModule : InjectionModule<TModule>
        {
            new FromInstanceContext(serviceProvider, module)
                .AddAllProperties()
                .ExceptFrom<InjectionModule<TModule>>(true)
                .Extract()
                .AddAllGetMethods()
                .ExceptFrom<InjectionModule<TModule>>(true)
                .Extract();
        }
    }
}
