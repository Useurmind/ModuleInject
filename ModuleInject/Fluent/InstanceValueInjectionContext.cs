using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class InstanceValueInjectionContext<IComponent, TComponent, IModule, TModule, TValue> : IInstanceValueInjectionContext<IComponent, TComponent, IModule, TModule, TValue>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        internal ValueInjectionContext Context { get; private set; }

        public IInstanceRegistrationContext<IComponent, TComponent, IModule, TModule> InstanceContext { get; private set; }
        public TValue Value { get { return (TValue)Context.Value; } }

        internal InstanceValueInjectionContext(InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instanceContext, ValueInjectionContext context)
        {
            InstanceContext = instanceContext;
            Context = context;
        }
    }
}
