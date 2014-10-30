using Microsoft.Practices.Unity;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Utility;
using System.Diagnostics.CodeAnalysis;

namespace ModuleInject.Fluent
{
    public class InterfaceRegistrationContext<IComponent, IModuleBase, TModule>
        where TModule : IModuleBase      
        where IModuleBase : IInjectionModule
    {
        internal ComponentRegistrationContext Context { get; private set; }

        public string ComponentName { get { return Context.ComponentName; } }

        internal InterfaceRegistrationContext(ComponentRegistrationContext context)
        {
            Context = context;            
        }

        internal InterfaceRegistrationContext<IComponent, IModuleBase, TModule> AddBehaviour<TBehaviour>()
            where TBehaviour : Unity.IInterceptionBehavior, new()
        {
            Context.AddBehaviour<TBehaviour>();
            
            return this;
        }
    }

    public class InterfaceRegistrationContext<IComponentBase, IModuleBase>
    {
        internal ComponentRegistrationContext Context { get; private set; }

        public string ComponentName { get { return Context.ComponentName; } }

        internal InterfaceRegistrationContext(ComponentRegistrationContext context)
        {
            Context = context;
        }

        internal InterfaceRegistrationContext<IComponentBase, IModuleBase> AddBehaviour<TBehaviour>()
            where TBehaviour : Unity.IInterceptionBehavior, new()
        {
            Context.AddBehaviour<TBehaviour>();

            return this;
        }
    }
}
