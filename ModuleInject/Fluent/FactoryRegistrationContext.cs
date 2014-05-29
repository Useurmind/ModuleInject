using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class FactoryRegistrationContext<IComponent, TComponent, IModule, TModule>
        where TModule : IModule
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        public string FactoryName { get; private set; }
        internal IUnityContainer Container { get; private set; }

        public FactoryRegistrationContext(string name, IUnityContainer container)
        {
            FactoryName = name;
            Container = container;
        }
    }
}
