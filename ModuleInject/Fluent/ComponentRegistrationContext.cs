using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
        where TModule : IModule        
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        public string ComponentName { get; private set; }
        internal IUnityContainer Container { get; private set; }

        public ComponentRegistrationContext(string name, IUnityContainer container)
        {
            ComponentName = name;
            Container = container;
        }
    }
}
