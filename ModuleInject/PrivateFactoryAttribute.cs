﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class PrivateFactoryAttribute : Attribute
    {
        
    }
}
