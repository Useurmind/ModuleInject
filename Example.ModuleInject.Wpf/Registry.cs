using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modularity.Registry;

namespace Example.ModuleInject.Wpf
{
    public class Registry : StandardRegistry
    {
        public Registry()
        {
            this.RegisterModule<IViewModelModule, ViewModelModule>();
            this.RegisterModule<IViewModule, ViewModule>();
        }
    }
}
