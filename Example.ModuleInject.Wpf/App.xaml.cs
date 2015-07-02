using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Example.ModuleInject.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var registry = new Registry();

            var viewModule = (IViewModule)registry.GetComponent(typeof(IViewModule));

            viewModule.Resolve(registry);

            viewModule.MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
