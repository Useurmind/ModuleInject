using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Provider
{
    /// <summary>
    /// Interface for classes that provide a service inside a service provider.
    /// </summary>
    public interface ISourceOfService
    {
        /// <summary>
        /// The key type/interface of the service.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Get an instance of the service.
        /// </summary>
        /// <returns>The service instance.</returns>
        object Get();
    }
}
