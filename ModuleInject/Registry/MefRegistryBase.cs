using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace ModuleInject.Registry
{
    using System.ComponentModel.Composition.Hosting;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Linq;
    using ModuleInject.Container;
    using ModuleInject.Container.Interface;
    using System.ComponentModel.Composition.Primitives;

    using ModuleInject.Container.Lifetime;


    public abstract class MefRegistryBase : Registry
    {
        private CompositionContainer compositionContainer;

        private AggregateCatalog aggregateCatalog;
        private bool isComposed;

        public MefRegistryBase()
        {
            aggregateCatalog = new AggregateCatalog();
            compositionContainer = new CompositionContainer(aggregateCatalog);
            isComposed = false;
        }

        internal override bool IsRegistered(Type type)
        {
            this.Compose();
            return base.IsRegistered(type);
        }

        internal override object GetComponent(Type type)
        {
            this.Compose();
            return base.GetComponent(type);
        }

        protected void AddCatalog(AssemblyCatalog catalog)
        {
            if (isComposed)
            {
                ExceptionHelper.ThrowFormatException(Errors.Registry_AlreadyComposedNoFurtherCatalogs);
            }
            aggregateCatalog.Catalogs.Add(catalog);
        }

        internal void Compose()
        {
            if (!isComposed)
            {
                this.compositionContainer.ComposeParts(this);
                isComposed = true;

                var imports = this.GetType().GetProperties().Where(p => p.HasCustomAttribute<ImportAttribute>());
                foreach (var import in imports)
                {
                    Register(import.PropertyType, import.GetValue(this, null));
                }
            }
        }
    }
}
