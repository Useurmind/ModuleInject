using System.Linq;

namespace ModuleInject.Container.Resolving
{
    using System;

    using ModuleInject.Container.Interface;

    public class ContainerReference : IResolvedValue
    {
        private string name;
        public Type Type { get; set; }
        private IDependencyContainer container;
        public ContainerReference(IDependencyContainer container, string name, Type type)
        {
            this.name = name;
            this.Type = type;
            this.container = container;
        }

        public object Resolve()
        {
            return this.container.Resolve(this.name, this.Type);
        }
    }
}
