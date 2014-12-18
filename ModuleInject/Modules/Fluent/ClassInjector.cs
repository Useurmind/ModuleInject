using System;
using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public class ClassInjector<IComponent, TComponent, IModule, TModule> :
        IClassInjector<IComponent, TComponent, IModule, TModule>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        private Action<IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> _injectInto;

        public ClassInjector(Action<IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> injectInto)
        {
            this._injectInto = injectInto;
        }

        public void InjectInto(IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> context)
        {
            this._injectInto(context);
        }
    }
}
