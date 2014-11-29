using System.Linq;

namespace ModuleInject.Decoration
{
    using System;

    /// <summary>
    /// Used to mark private components.
    /// Private components are the counterpart to public components.
    /// Public components are part of the interface of the module and must not to be marked explicitly.
    /// Private components on the other hand are not part of the interface and must be marked with this
    /// attribute in the module implementation.
    /// </summary>
    /// <remark>
    /// A private component does not need to have a private access modifier.
    /// Access Modifiers for setters can be set at your own discretion.
    /// </remark>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class PrivateComponentAttribute : Attribute
    {
        public PrivateComponentAttribute()
        {

        }
    }
}
