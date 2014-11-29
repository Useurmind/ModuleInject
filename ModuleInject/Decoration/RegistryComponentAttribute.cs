using System.Linq;

namespace ModuleInject.Decoration
{
    using System;

    /// <summary>
    /// States that a component is taken from the registry of the module.
    /// For this type of component no registration is allowed in the module.
    /// The components interface is automatically resolved from the registry 
    /// and an error is thrown if the registry does not contain a matching registration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class RegistryComponentAttribute : Attribute
    {

    }
}
