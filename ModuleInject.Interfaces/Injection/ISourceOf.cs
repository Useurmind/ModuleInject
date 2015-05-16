using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
	public interface ISourceOf<TIComponent>
	{
		/// <summary>
		/// Create an instance of the component.
		/// </summary>
		/// <returns>The created instance.</returns>
		TIComponent GetInstance();
	}
}
