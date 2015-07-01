using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Injection;
using ModuleInject.Interfaces.Injection;
using ModuleInject.Modularity;
using ModuleInject.Interfaces;

namespace Example.ModuleInject.Wpf
{
    public interface IViewModule : IModule
    {
        MainWindow MainWindow { get; }
    }

    public class ViewModule : InjectionModule<ViewModule>, IViewModule
    {
        [FromRegistry]
        public IViewModelModule ViewModelModule { get; set; }

        public MainWindow MainWindow
        {
            get
            {
                return GetSingleInstanceWithInject<MainWindow, MainWindow>(
                        (m, c) => c.DataContext = ViewModelModule.MainWindowVM);
            }
        }
    }
}
