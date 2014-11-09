using Microsoft.Practices.Unity;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Utility;
using System.Diagnostics.CodeAnalysis;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public class ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> : IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
        where TModule : IModule
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        internal RegistrationContext Context { get; private set; }

        public IRegistrationContext ReflectionContext
        {
            get
            {
                return Context;
            }
        }

        internal ComponentRegistrationContext(RegistrationContext context)
        {
            Context = context;
        }
    }
}
