using System.Linq;

namespace ModuleInject
{
    using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class RegistryComponentAttribute : Attribute
    {

    }
}
