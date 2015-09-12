using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Interfaces.Provider;

namespace ModuleInject.Provider.ServiceSources
{
    /// <summary>
    /// A service source that encapsulates a <see cref="PropertyInfo"/> and the corresponding instance
    /// to invoke it on.
    /// </summary>
    public class PropertyServiceSource : ISourceOfService
    {
        private object instance;
        private PropertyInfo propertyInfo;

        public PropertyServiceSource(object instance, PropertyInfo propertyInfo)
        {
            this.instance = instance;
            this.propertyInfo = propertyInfo;
        }

        public Type Type
        {
            get
            {
                return propertyInfo.PropertyType;
            }
        }

        public object Get()
        {
            return propertyInfo.GetValue(instance);
        }
    }
}
