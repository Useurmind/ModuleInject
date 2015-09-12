using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// Puzzling right...
    /// </summary>
	public interface IWrapInjectionRegister
	{
        /// <summary>
        /// Oh my god its wrapped.
        /// </summary>
		IInjectionRegister Register { get; }
	}
}
