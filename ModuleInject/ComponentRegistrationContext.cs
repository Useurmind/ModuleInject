using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject
{
    public class ComponentRegistrationContext<IComponent, TComponent, IModule>
        where TComponent : IComponent
    {
        public string ComponentName { get; internal set; }
        public IUnityContainer Container { get; internal set; }
    }
}
