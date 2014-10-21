using System.Linq;

namespace ModuleInject.Decoration
{
    using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class NonModulePropertyAttribute : Attribute
    {
        public NonModulePropertyAttribute()
        {
            
        }
    }
}
