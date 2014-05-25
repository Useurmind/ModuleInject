using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Utility
{
    [Serializable]
    public class ModuleInjectException : Exception
    {
        public ModuleInjectException() { }
        public ModuleInjectException(string message) : base(message) { }
        public ModuleInjectException(string message, Exception inner) : base(message, inner) { }
        protected ModuleInjectException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
