﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Interfaces.Provider;

namespace ModuleInject.Provider.ServiceSources
{
    /// <summary>
    /// A service source that encapsulates a <see cref="MethodInfo"/> and the corresponding instance
    /// to invoke it on.
    /// </summary>
    public class MethodServiceSource : ISourceOfService
    {
        private object instance;
        private MethodInfo methodInfo;

        public MethodServiceSource(object instance, MethodInfo methodInfo)
        {
            this.instance = instance;
            this.methodInfo = methodInfo;
        }

        public Type Type
        {
            get
            {
                return methodInfo.ReturnType;
            }
        }

        public object Get()
        {
            return methodInfo.Invoke(instance, null);
        }
    }
}
