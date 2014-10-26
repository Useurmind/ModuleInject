using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Interception
{
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    using Microsoft.Practices.Unity.InterceptionExtension;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class InterfaceInterceptorBuilder
    {
        public object CreateInterceptor(Type instanceInterface)
        {
            //InterfaceInterceptor interfaceInterceptor = new InterfaceInterceptor();
            //interfaceInterceptor.

            //Type interceptorType = null;

            //AssemblyName asmName = new AssemblyName();
            //asmName.Name = "InterfaceInterceptors";
            //AssemblyBuilder asmBuild = Thread.GetDomain().DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            //ModuleBuilder modBuild = asmBuild.DefineDynamicModule("InterfaceInterceptorModule", "InterfaceInterceptorModule.dll");      
            //TypeBuilder typeBuilder = modBuild.DefineType(string.Format("{0}Interceptor", instanceInterface.Name));

            //var properties = instanceInterface.GetProperties();
            //var methods = instanceInterface.GetMethods();

            //Dictionary<string, MethodBuilder> getMethods = new Dictionary<string, MethodBuilder>();
            //Dictionary<string, MethodBuilder> setMethods = new Dictionary<string, MethodBuilder>();
            
            //foreach (var method in methods)
            //{
            //    var methodBuilder = typeBuilder.DefineMethod(
            //        method.Name,
            //        MethodAttributes.Public,
            //        CallingConventions.Any,
            //        method.ReturnType,
            //        method.GetParameters().Select(x => x.ParameterType).ToArray());

            //    //methodBuilder.Is
            //}

            //foreach (var property in properties)
            //{
            //    var propertyBuilder = typeBuilder.DefineProperty(
            //        property.Name,
            //        PropertyAttributes.None,
            //        CallingConventions.Any,
            //        property.PropertyType,
            //        null);

            //    //propertyBuilder.
            //}

            //interceptorType = typeBuilder.CreateType();

            //return interceptorType;
            return null;
        }
    }
}
