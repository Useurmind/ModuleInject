using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public interface IFactory<TIComponent> : IInstanceProvider<TIComponent>
	{
		TIComponent Next();
	}

	public interface IInstanceProvider<TIComponent>
	{
		/// <summary>
		/// Create an instance of the component.
		/// </summary>
		/// <returns>The created instance.</returns>
		TIComponent GetInstance();
	}
}
