using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Linq;

namespace ModuleInject.Modularity.Registry
{
    public abstract class MefRegistryBase : StandardRegistry
    {
        private CompositionContainer compositionContainer;

        private AggregateCatalog aggregateCatalog;
        private bool isComposed;

        protected MefRegistryBase()
        {
            this.aggregateCatalog = new AggregateCatalog();
            this.compositionContainer = new CompositionContainer(this.aggregateCatalog);
            this.isComposed = false;
        }

        public override bool IsRegistered(Type type)
        {
            this.Compose();
            return base.IsRegistered(type);
        }

        public override object GetComponent(Type type)
        {
            this.Compose();
            return base.GetComponent(type);
        }

        protected void AddCatalog(ComposablePartCatalog catalog)
        {
            if (this.isComposed)
            {
                ExceptionHelper.ThrowFormatException(Errors.Registry_AlreadyComposedNoFurtherCatalogs);
            }
            this.aggregateCatalog.Catalogs.Add(catalog);
        }

        internal void Compose()
        {
            if (!this.isComposed)
            {
                this.compositionContainer.ComposeParts(this);
                this.isComposed = true;

                var imports = this.GetType().GetProperties().Where(p => p.HasCustomAttribute<ImportAttribute>());
                foreach (var import in imports)
                {
                    this.Register(import.PropertyType, import.GetValue(this, null));
                }
            }
        }
    }
}
