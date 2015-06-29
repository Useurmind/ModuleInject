using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Interception
{
    public static class InterceptedInjectionModuleExtensions
    {
        private static IDictionary<IInjectionRegister, IInterfaceInterceptor> interceptedRegisters;
        private static IInterceptorFactory interceptorFactory;

        private static IDictionary<IInjectionRegister, IInterfaceInterceptor> InterceptedRegisters
        {
            get
            {
                if (interceptedRegisters == null)
                {
                    interceptedRegisters = new Dictionary<IInjectionRegister, IInterfaceInterceptor>();
                }
                return interceptedRegisters;
            }
        }
        public static IModificationContext<TContext, TIComponent, TComponent> AddBehaviour<TContext, TIComponent, TComponent>(
            this IModificationContext<TContext, TIComponent, TComponent> sourceOf,
            IInterceptionBehaviour interceptionBehaviour)
            where TComponent : TIComponent
        {
            //IInterfaceInterceptor interceptor = null;
            //if(!interceptedRegisters.TryGetValue(sourceOf.Register, out interceptor))
            //{
            //    var newInterceptor = interceptorFactory.CreateInterceptor<TIComponent>();
            //    inter
            //    sourceOf.Change((ctx, comp) =>
            //    {
            //        ((InterfaceInterceptorBase<TIComponent>)(object)newInterceptor).SetInterceptedInstance(comp);
            //        return newInterceptor;
            //    });

            //    interceptedRegisters.Add(sourceOf.Register, newInterceptor)
            //}

            return sourceOf;
        }
    }
}
