using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Container.Interface;

    public class InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> : IGatherPostResolveAssemblers
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        private IList<IPostResolveAssembler> _postResolveAssemblers;

        public string ComponentName { get; private set; }
        internal IDependencyContainer Container { get; private set; }

        public InstanceRegistrationContext(string name, IDependencyContainer container)
        {
            ComponentName = name;
            Container = container;
            _postResolveAssemblers = new List<IPostResolveAssembler>();
        }

        public IList<IPostResolveAssembler> PostResolveAssemblers
        {
            get { return _postResolveAssemblers; }
        }

        public void AddAssembler(IPostResolveAssembler assembler)
        {
            _postResolveAssemblers.Add(assembler);
        }
    }
}
