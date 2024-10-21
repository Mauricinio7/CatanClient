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
                var animatedGrid = loginView.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDEOUTFROMTOPANIMATION);
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
                var animatedGrid = registerView.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)registerView.FindResource(Utilities.SLIDEOUTFROMTOPANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(1000);
                }


            }
            CurrentView = new Views.LoginView();
        }

        //TODO quit all methods and do it more general

        private async Task OcultVerifyAccountView()
        {
            if (currentView is Views.VerifyAccountView verifyAccountView)
            {
                var animatedGrid = verifyAccountView.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)verifyAccountView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(820);
                }


            }
            CurrentView = new Views.LoginView();
        }

        private async Task ShowVerifyAccountView(object accountDtoObj)
        {
            var accountDto = accountDtoObj as AccountDto; 
            if (accountDto == null)
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentView is Views.RegisterView registerView)
            {
                var animatedGrid = registerView.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDEOUTFROMTOPANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }

            if (currentView is Views.LoginView loginview)
            {
                var animatedGrid = loginview.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)loginview.FindResource(Utilities.SLIDEOUTFROMTOPANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }

            var verifyAccountView = new Views.VerifyAccountView(accountDto);

            CurrentView = verifyAccountView;
        }

        private async Task ShowGameLobby(object gameDtoObj)
        {
            var gameDto = gameDtoObj as GameDto;
            if (gameDto == null)
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentView is Views.CreateRoomView mainMenuView)
            {
                var animatedGrid = mainMenuView.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }

            var lobbyRoomView = new Views.GameLobbyView(gameDto);

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
                var animatedGrid = mainMenuView.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
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
                var animatedGrid = mainMenuView.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            CurrentView = new Views.LoginRoomView();
        }

        private async Task BackFromCreateRoom()
        {
            if (currentView is Views.CreateRoomView createRoomView)
            {
                var animatedGrid = createRoomView.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)createRoomView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
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
                var animatedGrid = loginRoomView.FindName(Utilities.ANIMATEDGRID) as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)loginRoomView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            CurrentView = new Views.MainMenuView();
        }

        

        


    }
}
