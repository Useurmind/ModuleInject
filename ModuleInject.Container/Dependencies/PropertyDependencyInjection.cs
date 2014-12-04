using System.Linq;

namespace ModuleInject.Container.Dependencies
{
    using System;

    using ModuleInject.Container.Interface;
    using ModuleInject.Container.Resolving;
    using ModuleInject.Container.Interface;

    public class PropertyDependencyInjection : IDependencyInjection
    {
        public string PropertyName { get; set; }

        public IResolvedValue ResolvedValue { get; set; }

        public void Resolve(object instance)
        {
            var value = this.ResolvedValue.Resolve();
            Type instanceType = instance.GetType();
            var propertyInfo = instanceType.GetProperty(this.PropertyName);
            propertyInfo.SetValue(instance, value, null);
        }
    }
}
