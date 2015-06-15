using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interception
{
    public interface IInterceptorFactory
    {
        TInterceptedInterface CreateInterceptor<TInterceptedInterface>();
    }
}
