using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IInjectionModule
    {
        void Resolve();

        object GetComponent(Type componentType, string componentName);

        IComponent GetComponent<IComponent>(string componentName);
    }
}
