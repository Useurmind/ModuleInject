using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example.ModuleInject.Wpf
{
    public interface IMainWindowVM
    {
        
    }

    public class MainWindowVM : IMainWindowVM 
    {
        public string Greeting { get; set; }
    }
}
