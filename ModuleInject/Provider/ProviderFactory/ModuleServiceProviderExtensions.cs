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
        public static IAllPropertiesContext FromModule<TModule>(this ServiceProvider serviceProvider, TModule module)
            where TModule : InjectionModule<TModule>
        {
            return new FromInstanceContext(serviceProvider, module)
                .AddAllProperties()
                .ExceptPropertiesFrom<InjectionModule<TModule>>(true);
        }
    }
}
