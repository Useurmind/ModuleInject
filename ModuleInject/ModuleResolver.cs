using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject
{
    public class ModuleResolver
    {
        public void Resolve<IModule>(IModule module, IUnityContainer container)
            where IModule : IInjectionModule
        {
            Type moduleType = typeof(IModule);

            if (!moduleType.IsInterface)
            {
                throw new ModuleInjectException("Modules must always have an Interface");
            }

            Type iinjectionModuleType = typeof(IInjectionModule);
            foreach (var propInfo in moduleType.GetProperties())
            {
                if (!propInfo.PropertyType.IsInterface)
                {
                    throw new ModuleInjectException("Dependencies in modules must always have an Interface");
                }

                if (propInfo.PropertyType.GetInterface(iinjectionModuleType.Name, false) != null)
                {
                    ResolveSubmodule<IModule>(module, container, propInfo);
                }
                else
                {
                    ResolveComponent<IModule>(module, container, moduleType, propInfo);
                }
            }
        }

        private void ResolveComponent<IModule>(IModule module, IUnityContainer container, Type moduleType, PropertyInfo propInfo) where IModule : IInjectionModule
        {
            Type propType = propInfo.PropertyType;
            string propName = propInfo.Name;

            if (!container.IsRegistered(propType, propName))
            {
                throw new ModuleInjectException(string.Format("The property '{0}' of the module interface '{1}' is not registered in the module.",
                                                    propName,
                                                    moduleType.Name));
            }
            var component = container.Resolve(propInfo.PropertyType, propInfo.Name);
            propInfo.SetValue(module, component, BindingFlags.NonPublic, null, null, null);
        }

        private void ResolveSubmodule<IModule>(IModule module, IUnityContainer container, PropertyInfo subModulePropInfo)
            where IModule : IInjectionModule
        {
            string submoduleName = subModulePropInfo.Name;
            IInjectionModule submodule = (IInjectionModule)subModulePropInfo.GetValue(module, null);

            submodule.Resolve();

            foreach (var propInfo in subModulePropInfo.PropertyType.GetProperties())
            {
                object subComponent = propInfo.GetValue(submodule, null);
                string subComponentName = string.Format("{0}.{1}", submoduleName, propInfo.Name);
                container.RegisterInstance(propInfo.PropertyType, subComponentName, subComponent);
            }
        }
    }
}
