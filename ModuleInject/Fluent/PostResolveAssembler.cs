using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class PostResolveAssembler<TComponent, IModule> : IPostResolveAssembler<TComponent, IModule>
        where IModule : IInjectionModule
    {
        private Action<TComponent, IModule> _assemble;

        public PostResolveAssembler(Action<TComponent, IModule> assemble)
        {
            _assemble = assemble;
        }

        public void Assemble(TComponent instance, IModule module)
        {
            _assemble(instance, module);
        }

        public void Assemble(object instance, IInjectionModule module)
        {
            Assemble((TComponent)instance, (IModule)module);
        }
    }
}
