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
        private UserControl overlayView;

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

        public UserControl OverlayView
        {
            get => overlayView;
            set
            {
                overlayView = value;
                OnPropertyChanged(nameof(OverlayView));
            }
        }

        public ICommand ShowRegisterViewCommand { get; }
        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowNeedHelpViewCommand { get; }
        public ICommand ShowChangeForgotPasswordViewCommand { get; }
        public ICommand ShowVerifyAccountViewCommand { get; }
        public ICommand OcultVerifyAccountViewCommand { get; }
        public ICommand ShowMainMenuViewCommand { get; }
        public ICommand ShowGuestMainMenuViewCommand { get; }
        public ICommand ShowMainMenuBackgroundViewCommand { get; }
        public ICommand ShowCreateRoomCommand { get; }
        public ICommand ShowLoginRoomCommand { get; }
        public ICommand BackToMainMenuCommand { get; }
        public ICommand BackToGuestMainMenuCommand { get; }
        public ICommand ShowGameLobbyCommand { get; }
        public ICommand ShowConfigureProfileCommand { get; }
        public ICommand ShowFriendsViewCommand { get; }
        public ICommand HideFriendsViewCommand { get; }
        public ICommand ShowFriendsRequestsViewCommand { get; }
        public ICommand HideFriendsRequestsViewCommand { get; }
        public ICommand ShowInviteFriendsViewCommand { get; }
        public ICommand HideInviteFriendsViewCommand { get; }
        public ICommand ShowScoreboardViewCommand { get; }
        public ICommand HideScoreboardViewCommand { get; }
        public ICommand ShowLoadingScreenCommand { get; }
        public ICommand HideLoadingScreenCommand { get; }

        public ICommand ShowGameScreenCommand { get; }

        public MainWindowsViewModel()
        {
            CurrentView = new Views.LoginView();
            BackgroundView = new Views.StartupBackground();

            ShowRegisterViewCommand = new RelayCommand(async () => await ShowRegisterView());
            ShowLoginViewCommand = new RelayCommand(async () => await ShowLoginView());
            ShowNeedHelpViewCommand = new RelayCommand(async () => await ShowNeedHelpView());
            ShowChangeForgotPasswordViewCommand = new RelayCommand(async (object email) => await ShowChangeForgotPasswordView(email));
            ShowVerifyAccountViewCommand = new RelayCommand(async (object accountDto) => await ShowVerifyAccountView(accountDto));
            OcultVerifyAccountViewCommand = new RelayCommand(async () => await OcultVerifyAccountView());
            ShowMainMenuViewCommand = new RelayCommand(ShowMainMenuView);
            ShowGuestMainMenuViewCommand = new RelayCommand(ShowGuestMainMenuView);
            ShowMainMenuBackgroundViewCommand = new RelayCommand(ShowMainMenuBackgroundView);
            ShowCreateRoomCommand = new RelayCommand(async () => await ShowCreateRoom());
            ShowFriendsViewCommand = new RelayCommand(ShowFriendsView);
            HideFriendsViewCommand = new RelayCommand(async () => await HideFriendsView());
            ShowFriendsRequestsViewCommand = new RelayCommand(ShowFriendsRequestsView);
            HideFriendsRequestsViewCommand = new RelayCommand(async () => await HideFriendsRequestsView());
            ShowInviteFriendsViewCommand = new RelayCommand(accesKey => ShowInviteFriendsView(accesKey));
            HideInviteFriendsViewCommand = new RelayCommand(async () => await HideInviteFriendsView());
            ShowScoreboardViewCommand = new RelayCommand(ShowScoreboardView);
            HideScoreboardViewCommand = new RelayCommand(async () => await HideScoreboardView());

            ShowLoginRoomCommand = new RelayCommand(async () => await ShowLoginRoom());
            BackToMainMenuCommand = new RelayCommand(async () => await BackToMainMenu());
            BackToGuestMainMenuCommand = new RelayCommand(async () => await BackToGuestMainMenu());
            ShowGameLobbyCommand = new RelayCommand(async (object gameDto) => await ShowGameLobby(gameDto));
            ShowConfigureProfileCommand = new RelayCommand(async (object accountDto) => await ShowConfigureProfile(accountDto));
            ShowLoadingScreenCommand = new RelayCommand(ShowLoadingScreen);
            HideLoadingScreenCommand = new RelayCommand(HideLoadingScreen);

            ShowGameScreenCommand = new RelayCommand(ShowGameScreen);

            Mediator.Register(Utilities.SHOWCONFIGUREPROFILE, args => ShowConfigureProfileCommand.Execute(args));
            Mediator.Register(Utilities.SHOW_CHANGE_FORGOT_PASSWORD, args => ShowChangeForgotPasswordViewCommand.Execute(args));
            Mediator.Register(Utilities.SHOWVERIFYACCOUNT, args => ShowVerifyAccountViewCommand.Execute(args));
            Mediator.Register(Utilities.SHOWGAMELOBBY, args => ShowGameLobbyCommand.Execute(args));
            Mediator.Register(Utilities.SHOWMAINMENU, args => ShowMainMenuViewCommand.Execute(null));
            Mediator.Register(Utilities.SHOW_GUEST_MAIN_MENU, args => ShowGuestMainMenuViewCommand.Execute(null));
            Mediator.Register(Utilities.SHOWMAINMENUBACKGROUND, args => ShowMainMenuBackgroundViewCommand.Execute(null));
            Mediator.Register(Utilities.OCULTVERIFYACCOUNT, args => OcultVerifyAccountViewCommand.Execute(null));
            Mediator.Register(Utilities.BACK_TO_MAIN_MENU_ROOM, args => BackToMainMenuCommand.Execute(null));
            Mediator.Register(Utilities.BACK_TO_GUEST_MAIN_MENU_ROOM, args => BackToGuestMainMenuCommand.Execute(null));
            Mediator.Register(Utilities.SHOW_INVITE_FRIENDS, args => ShowInviteFriendsViewCommand.Execute(args));
            Mediator.Register(Utilities.HIDE_INVITE_FRIENDS, args => HideInviteFriendsViewCommand.Execute(null));
            Mediator.Register(Utilities.SHOW_LOADING_SCREEN, args => ShowLoadingScreenCommand.Execute(null));
            Mediator.Register(Utilities.HIDE_LOADING_SCREEN, args => HideLoadingScreenCommand.Execute(null));

        }

        private void ShowGameScreen()
        {
            CurrentView = new Gameplay.Views.GameFrameView();
        }

        private void ShowLoadingScreen()
        {
            var loadingScreen = new Controls.LoadingScreen();
            OverlayView = loadingScreen;
        }

        private void HideLoadingScreen()
        {
            OverlayView = null;
        }

        private void ShowFriendsView()
        {
            var friendsView = new Views.FriendsView();
            OverlayView = friendsView;

        }

        private async Task HideFriendsView()
        {
            if (OverlayView is Views.FriendsView friendsView && friendsView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }
            OverlayView = null; 
        }

        private void ShowFriendsRequestsView()
        {
            var friendsView = new Views.FriendRequestView();
            OverlayView = friendsView;

        }

        private async Task HideFriendsRequestsView()
        {
            if (OverlayView is Views.FriendRequestView friendsView && friendsView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }
            OverlayView = null;
        }

        private void ShowInviteFriendsView(object accesKey)
        {
            if (!(accesKey is string accesKeyString))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var friendsView = new Views.InviteFriendView(accesKeyString);
            OverlayView = friendsView;

        }

        private async Task HideInviteFriendsView()
        {
            if (OverlayView is Views.InviteFriendView friendsView && friendsView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }
            OverlayView = null;
        }

        private void ShowScoreboardView()
        {
            var scoreboardView = new Views.ScoreboardView();
            OverlayView = scoreboardView;

        }

        private async Task HideScoreboardView()
        {
            if (OverlayView is Views.ScoreboardView scoreboardView && scoreboardView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDEOUTFROMLEFTANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }
            OverlayView = null;
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

        private async Task ShowNeedHelpView()
        {
            if (currentView is Views.LoginView loginView)
            {
                if (loginView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)loginView.FindResource(Utilities.SLIDEOUTFROMRIGHTANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(820);
                }


            }
            CurrentView = new Views.NeedHelpView();
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

        private async Task ShowChangeForgotPasswordView(object emailObj)
        {
            if (!(emailObj is string email))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentView is Views.NeedHelpView needHelpView)
            {
                if (needHelpView.FindName(Utilities.ANIMATEDGRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDEOUTFROMTOPANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            ChangeForgotPasswordView changeForgotPasswordView = new Views.ChangeForgotPasswordView(email);

            CurrentView = changeForgotPasswordView;
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

        private void ShowGuestMainMenuView()
        {
            CurrentView = new Views.GuestMainMenuView();
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

        private async Task BackToMainMenu()
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
            else if (currentView is Views.LoginRoomView loginRoomView)
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

        private async Task BackToGuestMainMenu()
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

            CurrentView = new Views.GuestMainMenuView();
        }

    }
}
