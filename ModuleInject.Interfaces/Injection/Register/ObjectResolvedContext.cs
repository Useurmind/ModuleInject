using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
	public class ObjectResolvedContext
	{
        public string ComponentName { get; set; }
		public Type ContextType { get; set; }
		public Type ComponentInterface { get; set; }
		public Type ComponentType { get; set; }
		public Object Instance { get; set; }
	}
}
