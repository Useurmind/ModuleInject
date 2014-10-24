using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class PostInjectionContext<IComponent, TComponent, IModule, TModule, IDependencyComponent>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        internal DependencyInjectionContext DependencyInjectionContext { get; private set; }
        public InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> InstanceContext { get; set; }
        public string DependencyName { get; set; }

        public PostInjectionContext(InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instanceContext,
            string dependencyName, Type dependencyType)
        {
            InstanceContext = instanceContext;
            DependencyName = dependencyName;
            DependencyInjectionContext = new DependencyInjectionContext(InstanceContext.Context.ComponentRegistrationContext, dependencyName, dependencyType);
        }
    }
}
