﻿using System;
using System.Linq;
using System.Linq.Expressions;

using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public static class RegistrationContextExtensions
    {
        /// <summary>
        /// Construct the component from a given instance.
        /// </summary>
        /// <typeparam name="IComponent">The interface of the component that is registered.</typeparam>
        /// <typeparam name="TComponent">The class that should be instantiated for the component that is registered.</typeparam>
        /// <param name="instance">The instance that should be registered.</param>
        /// <returns>A context for fluent injection into the component.</returns>
        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            Construct<IComponent, TComponent, IModule, TModule>(
            this IRegistrationContext<IComponent, IModule, TModule> registrationContext,
            TComponent instance)
            where TComponent : IComponent
        where TModule : InjectionModule<IModule, TModule>, IModule
        where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("registrationContext", registrationContext);

            registrationContext.Context.Construct(instance);

            return new ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>(registrationContext.Context);
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            Construct<IComponent, TComponent, IModule, TModule>(
            this IRegistrationContext<IComponent, IModule, TModule> registrationContext,
            Expression<Func<TModule, TComponent>> constructExpression)
            where TComponent : IComponent
        where TModule : InjectionModule<IModule, TModule>, IModule
            where IModule : Interfaces.IModule
        {
            CommonFunctions.CheckNullArgument("registrationContext", registrationContext);

            registrationContext.Context.Construct(constructExpression);

            return new ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>(registrationContext.Context);
        }
    }
}
