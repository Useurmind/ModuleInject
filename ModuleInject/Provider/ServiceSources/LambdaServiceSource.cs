using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces.Provider;

namespace ModuleInject.Provider.ServiceSources
{
    /// <summary>
    /// A service source that encapsulates a func that returns a service instance.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class LambdaServiceSource<TService> : ISourceOfService
    {
        private Func<TService> createService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createService">The func that will return the service.</param>
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
