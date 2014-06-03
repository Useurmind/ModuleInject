using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    internal class DependencyInjectionContext
    {
        public ComponentRegistrationContext ComponentContext { get; private set; }
        public string DependencyName { get; private set; }
        public Type DependencyType { get; private set; }

        public DependencyInjectionContext(ComponentRegistrationContext componentContext, string dependencyName, Type dependencyType)
        {
            ComponentContext = componentContext;
            DependencyName = dependencyName;
            DependencyType = dependencyType;
        }
    }
}
