using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Fluent
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
        where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("registrationContext", registrationContext);

            var contextImplementation = GetContextImplementation(registrationContext);

            contextImplementation.Context.Construct(instance);

            return new ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>(contextImplementation.Context);
        }

        public static IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            Construct<IComponent, TComponent, IModule, TModule>(
            this IRegistrationContext<IComponent, IModule, TModule> registrationContext,
            Expression<Func<TModule, TComponent>> constructExpression)
            where TComponent : IComponent
        where TModule : InjectionModule<IModule, TModule>, IModule
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("registrationContext", registrationContext);

            var contextImplementation = GetContextImplementation(registrationContext);

            contextImplementation.Context.Construct(constructExpression);

            return new ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>(contextImplementation.Context);
        }

        private static RegistrationContext<IComponent, IModule, TModule> GetContextImplementation<IComponent, IModule, TModule>(
            IRegistrationContext<IComponent, IModule, TModule> registrationContext)
                    where TModule : InjectionModule<IModule, TModule>, IModule
        where IModule : IInjectionModule
            {
            return (RegistrationContext<IComponent, IModule, TModule>)registrationContext;
            }

    }
}
