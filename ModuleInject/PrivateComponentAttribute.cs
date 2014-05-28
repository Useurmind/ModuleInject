﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class PrivateComponentAttribute : Attribute
    {
        public PrivateComponentAttribute()
        {

        }
    }
}
