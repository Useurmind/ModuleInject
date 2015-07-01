using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Injection;
using ModuleInject.Interfaces.Injection;
using ModuleInject.Interfaces;

namespace Example.ModuleInject.Wpf
{
    public interface IViewModelModule : IModule
    {
        IMainWindowVM MainWindowVM { get; }
    }

    public class ViewModelModule : InjectionModule<ViewModelModule>, IViewModelModule
    {
        public IMainWindowVM MainWindowVM
        {
            get
            {
                return
                    GetSingleInstanceWithInject<IMainWindowVM, MainWindowVM>(
                        (m, c) => c.Greeting = "This greeting is injected inside the module.");
            }
        }
    }
}
