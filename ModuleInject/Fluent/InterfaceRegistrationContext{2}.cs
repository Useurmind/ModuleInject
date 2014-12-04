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
    using ModuleInject.Module;
    
    internal class InterfaceRegistrationContext<IComponentBase, IModuleBase> : RegistrationContextBase, IInterfaceRegistrationContext<IComponentBase, IModuleBase>
    {
        internal InterfaceRegistrationContext(RegistrationContext context) : base(context)
        {
        }
    }
}
