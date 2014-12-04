using System.Linq;

namespace ModuleInject.Decoration
{
    using System;

    /// <summary>
    /// Used to mark private factory methods.
    /// Private factories are the counterpart to public factories.
    /// Public factories are part of the interface of the module and must not to be marked explicitly.
    /// Private factories on the other hand are not part of the interface and must be marked with this
    /// attribute in the module implementation.
    /// </summary>
    /// <remark>
    /// A private factory does not need to have a private access modifier.
    /// </remark>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class PrivateFactoryAttribute : Attribute
    {
        
    }
}
