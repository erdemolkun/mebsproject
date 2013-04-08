using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Threading;

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {            
            base.OnStartup(e);
        }
        protected override void OnActivated(EventArgs e)
        {            
            base.OnActivated(e);
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LoggerMebs.WriteToFile("Application_DispatcherUnhandledException\n"
                + e.Exception.Message);
            e.Handled = true;
        }
    }
}
