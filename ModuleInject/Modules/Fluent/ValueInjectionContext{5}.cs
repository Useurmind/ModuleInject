using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal class ValueInjectionContext<IComponent, TComponent, IModule, TModule, TValue> : IValueInjectionContext<IComponent, TComponent, IModule, TModule, TValue>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        internal ValueInjectionContext Context { get; private set; }

        public ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> ComponentContext { get; private set; }
        public TValue Value { get { return (TValue)this.Context.Value; } }

        internal ValueInjectionContext(ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext, ValueInjectionContext context)
        {
            this.ComponentContext = componentContext;
            this.Context = context;
        }
    }
}
