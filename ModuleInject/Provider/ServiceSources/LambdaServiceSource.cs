using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces.Provider;

namespace ModuleInject.Provider.ServiceSources
{
    public class LambdaServiceSource<TService> : ISourceOfService
    {
        private Func<TService> createService;

        public LambdaServiceSource(Func<TService> createService)
        {
            this.createService = createService;
        }

        public Type Type
        {
            get
            {
                return typeof(TService);
            }
        }

        public object Get()
        {
            return this.createService();
        }
    }
}
