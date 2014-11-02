﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public class InstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, IDependencyComponent> : IInstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, IDependencyComponent>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        internal DependencyInjectionContext DependencyInjectionContext { get; private set; }
        public InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> InstanceContext { get; set; }
        public string DependencyName { get; set; }

        public InstanceDependencyInjectionContext(InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instanceContext,
            string dependencyName, Type dependencyType)
        {
            InstanceContext = instanceContext;
            DependencyName = dependencyName;
            DependencyInjectionContext = new DependencyInjectionContext(InstanceContext.Context.ComponentRegistrationContext, dependencyName, dependencyType);
        }
    }
}
