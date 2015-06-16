using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
	public interface IWrapInjectionRegister
	{
		IInjectionRegister Register { get; }
	}
}
