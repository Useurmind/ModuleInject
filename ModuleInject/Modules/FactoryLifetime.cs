using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Modules
{
    using ModuleInject.Container.Interface;
    using ModuleInject.Container.Lifetime;

    internal class FactoryLifetime : DynamicLifetime
    {
        private InjectionModule module;
        
        public FactoryLifetime(InjectionModule module)
        {
            this.module = module;
        }

        public override void OnObjectResolved(ObjectResolvedContext context)
        {
            base.OnObjectResolved(context);
        }
    }
}
