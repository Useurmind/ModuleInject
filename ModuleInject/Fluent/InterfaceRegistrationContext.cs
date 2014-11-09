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

    public class InterfaceRegistrationContext<IComponent, IModuleBase, TModule> : IInterfaceRegistrationContext<IComponent, IModuleBase, TModule>
        where TModule : IModuleBase      
        where IModuleBase : IInjectionModule
    {
        internal RegistrationContext Context { get; private set; }

        public IRegistrationContext ReflectionContext
        {
            get
            {
                return Context;
            }
        }

        internal InterfaceRegistrationContext(RegistrationContext context)
        {
            Context = context;            
        }
    }

    public class InterfaceRegistrationContext<IComponentBase, IModuleBase> : IInterfaceRegistrationContext<IComponentBase, IModuleBase>
    {
        internal RegistrationContext Context { get; private set; }

        public IRegistrationContext ReflectionContext
        {
            get
            {
                return Context;
            }
        }

        internal InterfaceRegistrationContext(RegistrationContext context)
        {
            Context = context;
        }
    }
}
