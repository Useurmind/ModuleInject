using Microsoft.Practices.Unity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Container.Interface;

    internal class InstanceRegistrationContext
    {
        internal ComponentRegistrationContext ComponentRegistrationContext { get; private set; }
        public string ComponentName { get; private set; }
        internal IDependencyContainer Container { get; private set; }

        public InstanceRegistrationContext(string name, InjectionModule module, IDependencyContainer container, ComponentRegistrationTypes types)
        {
            this.ComponentRegistrationContext = new ComponentRegistrationContext(name, module, container, types, false);
            ComponentName = name;
            Container = container;
        }
    }
}
