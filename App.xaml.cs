using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
using System.Threading;
using Serilog;
using CatanClient.UIHelpers;

namespace CatanClient
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()  
                .WriteTo.Console()       
                .WriteTo.File(Utilities.LOGGER_FILE_DIRECTORY, rollingInterval: RollingInterval.Day) 
                .CreateLogger();


            Log.Information("La aplicación ha iniciado.");  

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("La aplicación ha finalizado.");  

            Log.CloseAndFlush();
            base.OnExit(e);
        }




    }
}
