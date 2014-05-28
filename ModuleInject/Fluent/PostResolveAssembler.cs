using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class PostResolveAssembler<TComponent, IModule, TModule> : IPostResolveAssembler<TComponent, IModule, TModule>
        where TModule : IModule
        where IModule : IInjectionModule
    {
        private Action<TComponent, TModule> _assemble;

        public PostResolveAssembler(Action<TComponent, TModule> assemble)
        {
            _assemble = assemble;
        }

        public void Assemble(TComponent instance, TModule module)
        {
            _assemble(instance, module);
        }

        public void Assemble(object instance, IInjectionModule module)
        {
            Assemble((TComponent)instance, (TModule)module);
        }
    }
}
