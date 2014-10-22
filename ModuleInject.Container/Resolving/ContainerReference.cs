using System.Linq;

namespace ModuleInject.Container.Resolving
{
    using System;

    using ModuleInject.Container.Interface;

    public class ContainerReference : IResolvedValue
    {
        private string name;
        private Type type;
        private IDependencyContainer container;
        public ContainerReference(IDependencyContainer container, string name, Type type)
        {
            this.name = name;
            this.type = type;
            this.container = container;
        }

        public object Resolve()
        {
            return this.container.Resolve(this.name, this.type);
        }
    }
}
