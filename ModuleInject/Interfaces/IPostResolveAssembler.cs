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

    public interface IPostResolveAssembler<TComponent, IModule> : IPostResolveAssembler
        where IModule : IInjectionModule
    {
        void Assemble(TComponent instance, IModule module);
    }
}
