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
        internal ComponentRegistrationContext Context { get; private set; }

        public string ComponentName { get { return Context.ComponentName; } }

        internal InterfaceRegistrationContext(ComponentRegistrationContext context)
        {
            Context = context;            
        }
    }

    public class InterfaceRegistrationContext<IComponentBase, IModuleBase> : IInterfaceRegistrationContext<IComponentBase, IModuleBase>
    {
        internal ComponentRegistrationContext Context { get; private set; }

        public string ComponentName { get { return Context.ComponentName; } }

        internal InterfaceRegistrationContext(ComponentRegistrationContext context)
        {
            Context = context;
        }
    }
}
