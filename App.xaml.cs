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
using CatanClient.Controls;
using CatanClient.Gameplay.ViewModels;
using System.IO;

namespace CatanClient
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        public static IContainer Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utilities.LOGGER_FOLDER);
            string logFilePath = Path.Combine(logDirectory, Utilities.LOGGER_FILE_NAME);

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information(Utilities.LogInfoStart(CultureInfo.CurrentCulture.Name));

            base.OnStartup(e);

            ContainerBuilder builder = new ContainerBuilder();

            ConfigureContainer(builder);

            Container = builder.Build();

            MainWindow mainWindow = Container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information(Utilities.LogInfoEnd(CultureInfo.CurrentCulture.Name));  

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
            builder.RegisterType<InvitePlayerCardViewModel>();
            builder.RegisterType<KickPlayerCardViewModel>();
            builder.RegisterType<VoteKickPlayerCardViewModel>();
            builder.RegisterType<FriendRequestViewModel>();
            builder.RegisterType<FriendsViewModel>();
            builder.RegisterType<InviteFriendViewModel>();
            builder.RegisterType<PlayerInRoomCardViewModel>();
            builder.RegisterType<PlayerInGameCardViewModel>();
            builder.RegisterType<KickPlayerWindowViewModel>();
            builder.RegisterType<VoteKickPlayerWindowViewModel>();
            builder.RegisterType<ExpelPlayerWindowViewModel>();
            builder.RegisterType<MainMenuViewModel>().AsSelf();
            builder.RegisterType<NeedHelpViewModel>().AsSelf();
            builder.RegisterType<ChangeForgotPasswordViewModel>().AsSelf();
            builder.RegisterType<GameFrameViewModel>().AsSelf();
            builder.RegisterType<TradeWindowViewModel>().AsSelf();

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
