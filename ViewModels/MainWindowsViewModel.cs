using CatanClient.AccountService;
using CatanClient.Commands;
using CatanClient.UIHelpers;
using CatanClient.Views;
using System;
using System.Collections.Generic;
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
        private UserControl _currentView;

        private UserControl _backgroundView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public UserControl BackgroundView
        {
            get => _backgroundView;
            set
            {
                _backgroundView = value;
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
        public ICommand BackFromCreateRoomCommand { get; }
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
            BackFromCreateRoomCommand = new RelayCommand(async () => await BackFromCreateRoom());
            ShowGameLobbyCommand = new RelayCommand(async () => await ShowGameLobby());

            Mediator.Register("ShowVerifyAccountView", args => ShowVerifyAccountViewCommand.Execute(args));
            Mediator.Register("ShowMainMenuView", args => ShowMainMenuViewCommand.Execute(null));
            Mediator.Register("ShowMainMenuBackgroundView", args => ShowMainMenuBackgroundViewCommand.Execute(null));
            Mediator.Register("OcultVerifyAccountView", args => OcultVerifyAccountViewCommand.Execute(null));
        }


        private async Task ShowRegisterView()
        {
            if (_currentView is Views.LoginView loginView)
            {
                var animatedGrid = loginView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)animatedGrid.FindResource("SlideOutFromTopAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(1000);
                }
            }

            CurrentView = new Views.RegisterView();
        }
        private async Task ShowLoginView()
        {
            if (_currentView is Views.RegisterView registerView)
            {
                var animatedGrid = registerView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)registerView.FindResource("SlideOutFromTopAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(1000);
                }


            }
            CurrentView = new Views.LoginView();
        }

        //TODO quit all methods and do it more general

        private async Task OcultVerifyAccountView()
        {
            if (_currentView is Views.VerifyAccountView verifyAccountView)
            {
                var animatedGrid = verifyAccountView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)verifyAccountView.FindResource("SlideOutFromRightAnimation");
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
                MessageBox.Show("Error al cargar la cuenta para verificar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_currentView is Views.RegisterView registerView)
            {
                var animatedGrid = registerView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)animatedGrid.FindResource("SlideOutFromTopAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }

            if (_currentView is Views.LoginView loginview)
            {
                var animatedGrid = loginview.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)loginview.FindResource("SlideOutFromTopAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }

            var verifyAccountView = new Views.VerifyAccountView(accountDto);

            CurrentView = verifyAccountView;
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
            if (_currentView is Views.MainMenuView mainMenuView)
            {
                var animatedGrid = mainMenuView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)mainMenuView.FindResource("SlideOutFromRightAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            CurrentView = new Views.CreateRoomView();
        }

        private async Task BackFromCreateRoom()
        {
            if (_currentView is Views.CreateRoomView createRoomView)
            {
                var animatedGrid = createRoomView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)createRoomView.FindResource("SlideOutFromRightAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            CurrentView = new Views.MainMenuView();
        }

        private async Task ShowGameLobby()
        {
            if (_currentView is Views.CreateRoomView mainMenuView)
            {
                var animatedGrid = mainMenuView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)mainMenuView.FindResource("SlideOutFromRightAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            CurrentView = new Views.GameLobbyView();
        }


    }
}
