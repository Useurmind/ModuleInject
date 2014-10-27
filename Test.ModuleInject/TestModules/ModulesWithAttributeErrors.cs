using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject;
    using global::ModuleInject.Registry;

    class ModuleWithNonMarkedPrivateComponent : InjectionModule<IEmptyModule, ModuleWithNonMarkedPrivateComponent>, IEmptyModule
    {
        public int UnmarkedPrivateComponent { get; set; }
    }

    class ModuleWithNonMarkedExternalComponent : InjectionModule<IEmptyModule, ModuleWithNonMarkedExternalComponent>, IEmptyModule
    {
        public int UnmarkedExternalComponent { get; set; }

        public ModuleWithNonMarkedExternalComponent()
        {
            UnmarkedExternalComponent = 1;
        }
    }

    class RegistryWithInt : MefRegistryBase
    {
        
    }

    class ModuleWithNonMarkedRegistryComponent : InjectionModule<IEmptyModule, ModuleWithNonMarkedRegistryComponent>, IEmptyModule
    {
        public int UnmarkedRegistryComponent { get; set; }

        public ModuleWithNonMarkedRegistryComponent()
        {
            Registry registry = new Registry();

            registry.Register(() => 1);

            Registry = registry;
        }
    }
}
