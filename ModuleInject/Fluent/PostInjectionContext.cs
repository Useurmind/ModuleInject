using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class PostInjectionContext<IComponent, TComponent, IModule, IDependencyComponent>
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        public InstanceRegistrationContext<IComponent, TComponent, IModule> InstanceContext { get; set; }
        public string DependencyName { get; set; }

        public PostInjectionContext(InstanceRegistrationContext<IComponent, TComponent, IModule> instanceContext,
            string dependencyName)
        {
            InstanceContext = instanceContext;
            DependencyName = dependencyName;
        }
    }
}
