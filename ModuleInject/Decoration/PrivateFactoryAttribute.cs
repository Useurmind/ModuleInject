using System.Linq;

namespace ModuleInject.Decoration
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class PrivateFactoryAttribute : Attribute
    {
        
    }
}
