using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public interface ISingleton<TComponent>
	{
		TComponent Instance { get; }
	}
}
