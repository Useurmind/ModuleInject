using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// Used in resolution hook function to describe what was resolved.
    /// </summary>
	public class ObjectResolvedContext
	{
        /// <summary>
        /// Name of the resolved component.
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// Type of the context aka module.
        /// </summary>
		public Type ContextType { get; set; }

        /// <summary>
        /// Interface under which the component is registered.
        /// </summary>
		public Type ComponentInterface { get; set; }

        /// <summary>
        /// The actual type of the component that is constructed.
        /// </summary>
		public Type ComponentType { get; set; }

        /// <summary>
        /// The instance that was created.
        /// </summary>
		public Object Instance { get; set; }
	}
}
