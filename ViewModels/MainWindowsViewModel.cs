using CatanClient.AccountService;
using CatanClient.Commands;
using CatanClient.GameService;
using CatanClient.UIHelpers;
using CatanClient.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CatanClient.ViewModels
{
    internal class MainWindowsViewModel : ViewModelBase
    {
        private UserControl currentView;

        private UserControl backgroundView;
        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public UserControl BackgroundView
        {
            get => backgroundView;
            set
            {
                backgroundView = value;
                OnPropertyChanged(nameof(BackgroundView));
            }
        }

        public ICommand ShowRegisterViewCommand { get; }
        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowVerifyAccountViewCommand { get; }
        public ICommand OcultVerifyAccountViewCommand { get; }
        public ICommand ShowMainMenuViewCommand { get; }
        public ICommand ShowMainMenuBackgroundViewCommand { get; }
        public ICommand ShowCreateRoomCommand { get; }
        public ICommand ShowLoginRoomCommand { get; }
        public ICommand BackFromCreateRoomCommand { get; }
        public ICommand BackFromLoginRoomCommand { get; }
        public ICommand ShowGameLobbyCommand { get; }
        public ICommand ShowConfigureProfileCommand { get; }

        public MainWindowsViewModel()
        {
            CurrentView = new Views.LoginView();
            BackgroundView = new Views.StartupBackground();

            ShowRegisterViewCommand = new RelayCommand(async () => await ShowRegisterView());
            ShowLoginViewCommand = new RelayCommand(async () => await ShowLoginView());
            ShowVerifyAccountViewCommand = new RelayCommand(async (object accountDto) => await ShowVerifyAccountView(accountDto));
            OcultVerifyAccountViewCommand = new RelayCommand(async () => await OcultVerifyAccountView());
            ShowMainMenuViewCommand = new RelayCommand(ShowMainMenuView);
            ShowMainMenuBackgroundViewCommand = new RelayCommand(ShowMainMenuBackgroundView);
            ShowCreateRoomCommand = new RelayCommand(async () => await ShowCreateRoom());
            
            ShowLoginRoomCommand = new RelayCommand(async () => await ShowLoginRoom());
            BackFromCreateRoomCommand = new RelayCommand(async () => await BackFromCreateRoom());
            BackFromLoginRoomCommand = new RelayCommand(async () => await BackFromLoginRoom());
            ShowGameLobbyCommand = new RelayCommand(async (object gameDto) => await ShowGameLobby(gameDto));
            ShowConfigureProfileCommand = new RelayCommand(async (object accountDto) => await ShowConfigureProfile(accountDto));

            Mediator.Register("ShowConfigureProfile", args => ShowConfigureProfileCommand.Execute(args));
            Mediator.Register(Utilities.SHOWVERIFYACCOUNT, args => ShowVerifyAccountViewCommand.Execute(args));
            Mediator.Register(Utilities.SHOWGAMELOBBY, args => ShowGameLobbyCommand.Execute(args));
            Mediator.Register(Utilities.SHOWMAINMENU, args => ShowMainMenuViewCommand.Execute(null));
            Mediator.Register(Utilities.SHOWMAINMENUBACKGROUND, args => ShowMainMenuBackgroundViewCommand.Execute(null));
            Mediator.Register(Utilities.OCULTVERIFYACCOUNT, args => OcultVerifyAccountViewCommand.Execute(null));
        }


        private async Task ShowRegisterView()
        {
            if (currentView is Views.LoginView loginView)
            {
                if (loginView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDEOUTFROMTOPANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(1000);
                }
            }

            CurrentView = new Views.RegisterView();
        }
        private async Task ShowLoginView()
        {
            if (currentView is Views.RegisterView registerView)
            {
                if (registerView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)registerView.FindResource(Utilities.SLIDEOUTFROMTOPANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(1000);
                }


            }
            CurrentView = new Views.LoginView();
        }


        private async Task OcultVerifyAccountView()
        {
            if (currentView is Views.VerifyAccountView verifyAccountView)
            {
                if (verifyAccountView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)verifyAccountView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(820);
                }


            }
            CurrentView = new Views.LoginView();
        }

        private async Task ShowVerifyAccountView(object accountDtoObj)
        {
            if (!(accountDtoObj is AccountDto accountDto))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentView is Views.RegisterView registerView)
            {
                if (registerView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDEOUTFROMTOPANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }

            if (currentView is Views.LoginView loginview)
            {
                if (loginview.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)loginview.FindResource(Utilities.SLIDEOUTFROMTOPANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }

            VerifyAccountView verifyAccountView = new Views.VerifyAccountView(accountDto);

            CurrentView = verifyAccountView;
        }

        private async Task ShowGameLobby(object gameDtoObj)
        {
            if (!(gameDtoObj is GameDto gameDto))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentView is Views.CreateRoomView mainMenuView)
            {
                if (mainMenuView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }

            GameLobbyView lobbyRoomView = new Views.GameLobbyView(gameDto);

            CurrentView = lobbyRoomView;
        }

        private void ShowMainMenuView()
        {
            CurrentView = new Views.MainMenuView();
        }

        private void ShowMainMenuBackgroundView()
        {
            BackgroundView = new Views.MainMenuBackgroundView();
        }

        private async Task ShowCreateRoom()
        {
            if (currentView is Views.MainMenuView mainMenuView)
            {
                if (mainMenuView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            CurrentView = new Views.CreateRoomView();
        }

        private async Task ShowLoginRoom()
        {
            if (currentView is Views.MainMenuView mainMenuView)
            {
                if (mainMenuView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            CurrentView = new Views.LoginRoomView();
        }


        private async Task ShowConfigureProfile(object account)
        {
            if (!(account is AccountDto accountDto))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentView is Views.MainMenuView mainMenuView)
            {
                if (mainMenuView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }

            ConfigureProfileView configureProfileView = new Views.ConfigureProfileView(accountDto);

            CurrentView = configureProfileView;
        }

        private async Task BackFromCreateRoom()
        {
            if (currentView is Views.CreateRoomView createRoomView)
            {
                if (createRoomView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)createRoomView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            CurrentView = new Views.MainMenuView();
        }

        private async Task BackFromLoginRoom()
        {
            if (currentView is Views.LoginRoomView loginRoomView)
            {
                if (loginRoomView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)loginRoomView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            CurrentView = new Views.MainMenuView();
        }

        

        


    }
}
