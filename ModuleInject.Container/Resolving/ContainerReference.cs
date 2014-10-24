using System.Linq;

namespace ModuleInject.Container.Resolving
{
    using System;

    using ModuleInject.Container.Interface;

    public class ContainerReference : IResolvedValue
    {
        private string name;

        private Func<IDependencyContainer> getContainer;
        private IDependencyContainer container;
        
        public Type Type { get; set; }

        private IDependencyContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = getContainer();
                }
                return container;
            }
        }

        private ContainerReference(string name, Type type)
        {
            this.name = name;
            this.Type = type;
        }
 
        public ContainerReference(IDependencyContainer container, string name, Type type) :this(name, type)
        {
            this.container = container;
        }

        public ContainerReference(Func<IDependencyContainer> getContainer, string name, Type type)
            : this(name, type)
        {
            this.getContainer = getContainer;
        }

        public object Resolve()
        {
            return this.Container.Resolve(this.name, this.Type);
        }
    }
}
