using Microsoft.Practices.Unity;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;
using ModuleInject.Interception;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Utility;
using System.Diagnostics.CodeAnalysis;

namespace ModuleInject.Fluent
{
    public class ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
        where TModule : IModule
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        internal ComponentRegistrationContext Context { get; private set; }

        /// <summary>
        /// The name of the component which is configured by this context.
        /// </summary>
        public string ComponentName { get { return Context.ComponentName; } }

        internal ComponentRegistrationContext(ComponentRegistrationContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Add a behavior of the given type.
        /// </summary>
        /// <typeparam name="TBehaviour">The type of the behavior to add.</typeparam>
        /// <returns>The current context of the fluent API.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This API is by design statically typed")]
        internal ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> AddBehaviour<TBehaviour>()
            where TBehaviour : Unity.IInterceptionBehavior, new()
        {
            Context.AddBehaviour<TBehaviour>();

            return this;
        }
    }
}
