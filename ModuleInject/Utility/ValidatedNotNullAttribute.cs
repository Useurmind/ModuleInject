using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Utility
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
