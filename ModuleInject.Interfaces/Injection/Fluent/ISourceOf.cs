using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// Simple interface that represents a source of a component.
    /// </summary>
    /// <typeparam name="TIComponent">The interface of the component that the source produces.</typeparam>
	public interface ISourceOf<TIComponent>
	{
		/// <summary>
		/// Create an instance of the component.
		/// </summary>
		/// <returns>The created instance.</returns>
		TIComponent Get();
	}
}
