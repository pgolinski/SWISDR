using Autofac;
using SWISDR.Core;
using SWISDR.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using IContainer = Autofac.IContainer;

namespace SWISDR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IContainer _container;
        private void OnStartup(object sender, StartupEventArgs args)
        {
            _container = BuildContainer();

            var window = _container.Resolve<MainWindow>();
            window.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            _container.Dispose();
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new UIModule());

            return builder.Build();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
