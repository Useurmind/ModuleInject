using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Module
{
    using ModuleInject.Container.Interface;
    using ModuleInject.Container.Lifetime;

    internal class ComponentLifetime : SingletonLifetime
    {
        private InjectionModule module;

        public ComponentLifetime(InjectionModule module)
        {
            this.module = module;
        }

        public override void OnObjectResolved(ObjectResolvedContext context)
        {
            base.OnObjectResolved(context);

            this.module.OnComponentResolved(context);
        }
    }
}
