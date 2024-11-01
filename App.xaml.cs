﻿using System;
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
using CatanClient.Controls;

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

            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            Container = builder.Build();

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
            builder.RegisterType<AccountServiceClient>().As<IAccountServiceClient>().SingleInstance();
            builder.RegisterType<GameServiceClient>().As<IGameServiceClient>().SingleInstance();
            builder.RegisterType<ChatServiceClient>().As<IChatServiceClient>().SingleInstance();
            builder.RegisterType<ProfileServiceClient>().As<IProfileServiceClient>().SingleInstance();
            builder.RegisterType<GuestAccountServiceClient>().As<IGuestAccountServiceClient>().SingleInstance();
            builder.RegisterType<ProfileSingleton>().As<IProfileSingleton>().SingleInstance();

            builder.RegisterType<ServiceManager>().AsSelf();

            builder.RegisterType<CreateRoomViewModel>().AsSelf();
            builder.RegisterType<GameLobbyViewModel>().AsSelf();
            builder.RegisterType<LoginRoomViewModel>().AsSelf();
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<MainWindowsViewModel>().AsSelf();
            builder.RegisterType<RegisterViewModel>().AsSelf();
            builder.RegisterType<VerifyAccountViewModel>().AsSelf();
            builder.RegisterType<ConfigureProfileViewModel>().AsSelf();
            builder.RegisterType<EditProfileWindowViewModel>().AsSelf();
            builder.RegisterType<EditPasswordWindowViewModel>().AsSelf();
            builder.RegisterType<VerifyAccountChangeWindowViewModel>().AsSelf();
            builder.RegisterType<AddFriendWindowViewModel>().AsSelf();
            builder.RegisterType<ScoreboardViewModel>().AsSelf();
            builder.RegisterType<FriendPlayerCardViewModel>();
            builder.RegisterType<FriendRequestPlayerCardViewModel>();
            builder.RegisterType<MainMenuViewModel>().AsSelf();

            builder.RegisterType<CreateRoomView>().AsSelf();
            builder.RegisterType<GameLobbyView>().AsSelf();
            builder.RegisterType<LoginRoomView>().AsSelf();
            builder.RegisterType<LoginView>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<RegisterView>().AsSelf();
            builder.RegisterType<VerifyAccountView>().AsSelf();
            builder.RegisterType<ConfigureProfileView>().AsSelf();
            builder.RegisterType<EditProfileWindow>().AsSelf();
            builder.RegisterType<EditPasswordWindow>().AsSelf();
            builder.RegisterType<VerifyAccountChangeWindow>().AsSelf();
            builder.RegisterType<AddFriendWindow>().AsSelf();
            builder.RegisterType<ScoreboardView>().AsSelf();
            builder.RegisterType<MainMenuView>().AsSelf();
        }




    }
}
