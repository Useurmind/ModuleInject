using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Utility
{
    public class MethodCallArgument
    {
        public Type ArgumentType { get; set; }
        public string ResolvePath { get; set; }
        public object Value { get; set; }

    }
}
