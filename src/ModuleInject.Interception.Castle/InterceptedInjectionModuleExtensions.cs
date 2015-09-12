using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Interception.Castle
{
    public static class InterceptedInjectionModuleExtensions
    {
        private static ProxyGenerator generator;
        private static IDictionary<IInjectionRegister, IList<Func<object, IInterceptor>>> registeredInterceptors;

        private static IDictionary<IInjectionRegister, IList<Func<object, IInterceptor>>> RegisteredInterceptors
        {
            get
            {
                if (registeredInterceptors == null)
                {
                    registeredInterceptors = new Dictionary<IInjectionRegister, IList<Func<object, IInterceptor>>>();
                }
                return registeredInterceptors;
            }
        }

        private static ProxyGenerator Generator
        {
            get
            {
                if (generator == null)
                {
                    generator = new ProxyGenerator();
                }
                return generator;
            }
        }

        /// <summary>
        /// Add an interceptor to a component of a module.
        /// </summary>
        /// <typeparam name="TContext">The type of the module.</typeparam>
        /// <typeparam name="TIComponent">The interface of the component.</typeparam>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <param name="sourceOf"></param>
        /// <param name="createInterceptor">A func that takes the instance of the context and returns an interceptor.</param>
        /// <returns></returns>
        public static IModificationContext<TContext, TIComponent, TComponent> AddInterceptor<TContext, TIComponent, TComponent>(
            this IModificationContext<TContext, TIComponent, TComponent> sourceOf,
            Func<TContext, IInterceptor> createInterceptor)
            where TIComponent : class
            where TComponent : TIComponent
        {
            var injectionRegister = sourceOf.Register;
            bool wasCreated = false;
            IList<Func<object, IInterceptor>> interceptorConstructors = GetInterceptorList(injectionRegister, out wasCreated);
            interceptorConstructors.Add((object o) => createInterceptor((TContext)o));

            if (wasCreated)
            {
                return sourceOf.Change((ctx, comp) =>
                {
                    var lambdaInterceptorList = GetInterceptorList(injectionRegister, out wasCreated);

                    IInterceptor[] interceptors = new IInterceptor[lambdaInterceptorList.Count];

                    for (int i = 0; i < lambdaInterceptorList.Count; i++)
                    {
                        interceptors[i] = lambdaInterceptorList[i](ctx);
                    }

                    return Generator.CreateInterfaceProxyWithTarget<TIComponent>(comp, interceptors);
                });
            }

            return sourceOf;
        }

        private static IList<Func<object, IInterceptor>> GetInterceptorList(IInjectionRegister injectionRegister, out bool wasCreated)
        {
            wasCreated = false;
            IList<Func<object, IInterceptor>> interceptors = null;

            if (!RegisteredInterceptors.TryGetValue(injectionRegister, out interceptors))
            {
                interceptors = new List<Func<object, IInterceptor>>();
                RegisteredInterceptors.Add(injectionRegister, interceptors);
                wasCreated = true;
            }

            return interceptors;
        }
    }
}
