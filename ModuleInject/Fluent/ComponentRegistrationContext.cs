using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class ComponentRegistrationContext<IComponent, TComponent, IModule>
        where TComponent : IComponent
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
