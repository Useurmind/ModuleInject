using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Exceptions;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection
{
    public static class InjectionRegisterExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void CheckTypes<TModule, TIComponent>(this IInjectionRegister injectionRegister)
        {
            injectionRegister.CheckTypes(typeof(TModule), typeof(TIComponent));
        }

        public static void CheckTypes(this IInjectionRegister injectionRegister, Type moduleType, Type componentInterface)
        {
            if(injectionRegister.ContextType != moduleType ||
                injectionRegister.ComponentInterface != componentInterface)
            {
                ExceptionHelper.ThrowFormatException(Errors.InjectionRegister_TypeMismatch,
                    injectionRegister.ContextType.Name,
                    injectionRegister.ComponentInterface.Name,
                    moduleType.Name,
                    componentInterface.Name);
            }
        }

        //public static void CheckTypesDerive(this IInjectionRegister injectionRegister, Type moduleType, Type componentInterface)
        //{
        //    if (injectionRegister.ContextType != moduleType ||
        //        injectionRegister.ComponentInterface != componentInterface)
        //    {
        //        ExceptionHelper.ThrowFormatException(Errors.InjectionRegister_TypeMismatch,
        //            injectionRegister.ContextType.Name,
        //            injectionRegister.ComponentInterface.Name,
        //            moduleType.Name,
        //            componentInterface.Name);
        //    }
        //}
    }
}
