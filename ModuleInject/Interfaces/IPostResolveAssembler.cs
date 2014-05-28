using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IPostResolveAssembler
    {
        void Assemble(object instance, IInjectionModule module);
    }

    public interface IPostResolveAssembler<TComponent, IModule, TModule> : IPostResolveAssembler
        where TModule : IModule
        where IModule : IInjectionModule
    {
        void Assemble(TComponent instance, TModule module);
    }
}
