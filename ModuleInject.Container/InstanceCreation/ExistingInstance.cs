using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.InstanceCreation
{
    public class ExistingInstance : IInstanceCreation
    {
        public ExistingInstance(object instance)
        {
            this.instance = instance;
        }

        public object Resolve(Type actualType)
        {
            return instance;
        }

        public object instance { get; set; }
    }
}
