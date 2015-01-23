using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal class RegistrationContext<IComponent, IModule, TModule> : RegistrationContextBase, IRegistrationContext<IComponent, IModule, TModule>
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        internal RegistrationContext(RegistrationContext context) : base(context)
        {
        }

        /// <summary>
        /// Construct the component from a given type.
        /// </summary>
        /// <typeparam name="TComponent">The class that should be instantiated for the component that is registered.</typeparam>
        /// <returns>A context for fluent injection into the component.</returns>
        public IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            Construct<TComponent>()
            where TComponent : IComponent
        {
            this.Context.Construct(typeof(TComponent));

            return new ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>(this.Context);
        }
    }
}
