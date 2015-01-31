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
        public IValueInjectionContext Context { get; private set; }

        public IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> ComponentContext { get; private set; }
        public TValue Value { get { return (TValue)this.Context.Value; } }

        internal ValueInjectionContext(IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext, IValueInjectionContext context)
        {
            this.ComponentContext = componentContext;
            this.Context = context;
        }
    }
}
