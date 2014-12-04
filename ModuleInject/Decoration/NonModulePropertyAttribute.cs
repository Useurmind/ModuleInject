using System.Linq;

namespace ModuleInject.Decoration
{
    using System;

    /// <summary>
    /// Used to mark properties that should be ignored by the modules resolution process.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class NonModulePropertyAttribute : Attribute
    {
        public NonModulePropertyAttribute()
        {
            
        }
    }
}
