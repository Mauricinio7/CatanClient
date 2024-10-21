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
using Autofac;
using CatanClient.Views;
using CatanClient.Services;
using CatanClient.Singleton;
using CatanClient.ViewModels;

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

        public static IContainer Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()  
                .WriteTo.Console()       
                .WriteTo.File(Utilities.LOGGER_FILE_DIRECTORY, rollingInterval: RollingInterval.Day) 
                .CreateLogger();


            Log.Information("La aplicación ha iniciado.");  

            base.OnStartup(e);

            // Configurar el contenedor de Autofac
            var builder = new ContainerBuilder();

            // Registrar los servicios, ViewModels y vistas
            ConfigureContainer(builder);

            // Construir el contenedor de Autofac
            Container = builder.Build();

            // Resolver la ventana principal desde el contenedor de Autofac
            var mainWindow = Container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("La aplicación ha finalizado.");  

            Log.CloseAndFlush();
            base.OnExit(e);
        }

        private void ConfigureContainer(ContainerBuilder builder)
        {
            // Registrar los ViewModels
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<MainWindowsViewModel>().AsSelf();

            // Registrar los servicios como interfaces
            builder.RegisterType<AccountServiceClient>().As<IAccountServiceClient>().SingleInstance();
            builder.RegisterType<ProfileSingleton>().As<IProfileSingleton>().SingleInstance();

            // Registrar las vistas
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<LoginView>().AsSelf();
        }




    }
}
