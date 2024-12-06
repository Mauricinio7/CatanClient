using CatanClient.AccountService;
using CatanClient.Commands;
using CatanClient.Gameplay.Helpers;
using CatanClient.Gameplay.Views;
using CatanClient.GameService;
using CatanClient.Properties;
using CatanClient.UIHelpers;
using CatanClient.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
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
        private SoundPlayer soundPlayer;
        public ICommand ShowRegisterViewCommand { get; set; }
        public ICommand ShowLoginViewCommand { get; set; }
        public ICommand ShowNeedHelpViewCommand { get; set; }
        public ICommand ShowChangeForgotPasswordViewCommand { get; set; }
        public ICommand ShowVerifyAccountViewCommand { get; set; }
        public ICommand OcultVerifyAccountViewCommand { get; set; }
        public ICommand ShowMainMenuViewCommand { get; set; }
        public ICommand ShowGuestMainMenuViewCommand { get; set; }
        public ICommand ShowMainMenuBackgroundViewCommand { get; set; }
        public ICommand ShowCreateRoomCommand { get; set; }
        public ICommand ShowLoginRoomCommand { get; set; }
        public ICommand BackToMainMenuCommand { get; set; }
        public ICommand BackToGuestMainMenuCommand { get; set;   }
        public ICommand ShowGameLobbyCommand { get; set; }
        public ICommand ShowConfigureProfileCommand { get; set; }
        public ICommand ShowFriendsViewCommand { get; set; }
        public ICommand HideFriendsViewCommand { get; set; }
        public ICommand ShowFriendsRequestsViewCommand { get; set; }
        public ICommand HideFriendsRequestsViewCommand { get; set; }
        public ICommand ShowInviteFriendsViewCommand { get; set; }
        public ICommand HideInviteFriendsViewCommand { get; set; }
        public ICommand ShowScoreboardViewCommand { get; set; }
        public ICommand HideScoreboardViewCommand { get; set; }
        public ICommand ShowLoadingScreenCommand { get; set; }
        public ICommand HideLoadingScreenCommand {get; set; }
        public ICommand ShowDiceAnimationCommand { get; set; }
        public ICommand ShowWinAnimationCommand { get; set; }
        public ICommand ShowDiceResultAnimationCommand { get; set; }
        public ICommand ShowGameScreenCommand { get; set; }



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

        public MainWindowsViewModel()
        {
            
            InicializateViews();
            InicializateCommands();
            InicializateAsyncCommands();
            InicializateMediatorRegisters();
            InicializateMusic();
            PlayLoginMusic();

        }

        private void InicializateMusic()
        {
            try
            {
                Uri uri = new Uri(Utilities.LOGIN_MUSIC);

                System.IO.Stream resourceStream = Application.GetResourceStream(uri)?.Stream;

                soundPlayer = new SoundPlayer(resourceStream);
            }
            catch (UriFormatException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
            }
        }

        public void PlayLoginMusic()
        {
            try
            {
                soundPlayer.PlayLooping();
            }
            catch (UriFormatException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
            }
        }

        public void ChangeMusic(string newMusicUri)
        {
            try
            {
                soundPlayer?.Stop();

                Uri uri = new Uri(newMusicUri);
                System.IO.Stream resourceStream = Application.GetResourceStream(uri)?.Stream;

                if (resourceStream != null)
                {
                    soundPlayer = new SoundPlayer(resourceStream);
                    soundPlayer.PlayLooping();
                }
            }
            catch (UriFormatException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
            }
        }


        private void InicializateViews()
        {
            CurrentView = new Views.LoginView();
            BackgroundView = new Views.StartupBackground();
        }

        private void InicializateMediatorRegisters()
        {
            Mediator.Register(Utilities.SHOW_CONFIGURE_PROFILE, args => ShowConfigureProfileCommand.Execute(args));
            Mediator.Register(Utilities.SHOW_CHANGE_FORGOT_PASSWORD, args => ShowChangeForgotPasswordViewCommand.Execute(args));
            Mediator.Register(Utilities.SHOW_VERIFY_ACCOUNT, args => ShowVerifyAccountViewCommand.Execute(args));
            Mediator.Register(Utilities.SHOW_GAME_LOBBY, args => ShowGameLobbyCommand.Execute(args));
            Mediator.Register(Utilities.SHOW_MAIN_MENU, args => ShowMainMenuViewCommand.Execute(null));
            Mediator.Register(Utilities.SHOW_GUEST_MAIN_MENU, args => ShowGuestMainMenuViewCommand.Execute(null));
            Mediator.Register(Utilities.SHOW_MAIN_MENU_BACKGROUND, args => ShowMainMenuBackgroundViewCommand.Execute(null));
            Mediator.Register(Utilities.OCULT_VERIFY_ACCOUNT, args => OcultVerifyAccountViewCommand.Execute(null));
            Mediator.Register(Utilities.BACK_TO_MAIN_MENU_ROOM, args => BackToMainMenuCommand.Execute(null));
            Mediator.Register(Utilities.BACK_TO_GUEST_MAIN_MENU_ROOM, args => BackToGuestMainMenuCommand.Execute(null));
            Mediator.Register(Utilities.SHOW_INVITE_FRIENDS, args => ShowInviteFriendsViewCommand.Execute(args));
            Mediator.Register(Utilities.HIDE_INVITE_FRIENDS, args => HideInviteFriendsViewCommand.Execute(null));
            Mediator.Register(Utilities.SHOW_LOADING_SCREEN, args => ShowLoadingScreenCommand.Execute(null));
            Mediator.Register(Utilities.HIDE_LOADING_SCREEN, args => HideLoadingScreenCommand.Execute(null));
            Mediator.Register(Utilities.SHOW_GAME_SCREEN, args => ShowGameScreenCommand.Execute(args));
            Mediator.Register(Utilities.SHOW_ROLL_DICE_ANIMATION, args => ShowDiceAnimationCommand.Execute(args));
            Mediator.Register(Utilities.SHOW_WIN_ANIMATION, args => ShowWinAnimationCommand.Execute(args));
            Mediator.Register(Utilities.SHOW_DICE_RESULT_ANIMATION, args => ShowDiceResultAnimationCommand.Execute(args));
            Mediator.Register(Utilities.SHOW_SCORE_FRAME, args => ShowScoreboardViewCommand.Execute(args));
        }

        private void InicializateAsyncCommands()
        {
            ShowRegisterViewCommand = new RelayCommand(async () => await ShowRegisterView());
            ShowLoginViewCommand = new RelayCommand(async () => await ShowLoginView());
            ShowNeedHelpViewCommand = new RelayCommand(async () => await ShowNeedHelpView());
            ShowChangeForgotPasswordViewCommand = new RelayCommand(async (object email) => await ShowChangeForgotPasswordView(email));
            ShowVerifyAccountViewCommand = new RelayCommand(async (object accountDto) => await ShowVerifyAccountView(accountDto));
            OcultVerifyAccountViewCommand = new RelayCommand(async () => await OcultVerifyAccountView());
            ShowCreateRoomCommand = new RelayCommand(async () => await ShowCreateRoom());
            HideFriendsViewCommand = new RelayCommand(async () => await HideFriendsView());
            HideFriendsRequestsViewCommand = new RelayCommand(async () => await HideFriendsRequestsView());
            HideInviteFriendsViewCommand = new RelayCommand(async () => await HideInviteFriendsView());
            HideScoreboardViewCommand = new RelayCommand(async () => await HideScoreboardView());
            ShowLoginRoomCommand = new RelayCommand(async () => await ShowLoginRoom());
            BackToMainMenuCommand = new RelayCommand(async () => await BackToMainMenu());
            BackToGuestMainMenuCommand = new RelayCommand(async () => await BackToGuestMainMenu());
            ShowGameLobbyCommand = new RelayCommand(async (object gameDto) => await ShowGameLobby(gameDto));
            ShowConfigureProfileCommand = new RelayCommand(async (object accountDto) => await ShowConfigureProfile(accountDto));
        }

        private void InicializateCommands()
        {
            ShowMainMenuViewCommand = new RelayCommand(ShowMainMenuView);
            ShowGuestMainMenuViewCommand = new RelayCommand(ShowGuestMainMenuView);
            ShowMainMenuBackgroundViewCommand = new RelayCommand(ShowMainMenuBackgroundView);
            ShowFriendsViewCommand = new RelayCommand(ShowFriendsView);
            ShowFriendsRequestsViewCommand = new RelayCommand(ShowFriendsRequestsView);
            ShowInviteFriendsViewCommand = new RelayCommand(accesKey => ShowInviteFriendsView(accesKey));
            ShowScoreboardViewCommand = new RelayCommand(ShowScoreboardView);
            ShowDiceAnimationCommand = new RelayCommand(result => ShowDiceAnimation(result));
            ShowWinAnimationCommand= new RelayCommand(winner => ShowWinAnimation(winner));
            ShowDiceResultAnimationCommand = new RelayCommand(result => ShowDiceResultAnimation(result));
            ShowLoadingScreenCommand = new RelayCommand(ShowLoadingScreen);
            HideLoadingScreenCommand = new RelayCommand(HideLoadingScreen);
            ShowGameScreenCommand = new RelayCommand(gameDto => ShowGameScreen(gameDto));
        }

        private void ShowWinAnimation(object parameter)
        {
            if (!(parameter is string winnerName))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            OverlayView = new WinnerAnimationView(winnerName);
        }

        private void ShowDiceAnimation(object result)
        {
            if (!(result is int diceResult))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DiceRollAnimationView diceAnimationView = new DiceRollAnimationView(diceResult);
            diceAnimationView.AnimationCompleted += DiceAnimationView_AnimationCompleted;
            OverlayView = diceAnimationView;
        }

        private void DiceAnimationView_AnimationCompleted(object sender, EventArgs e)
        {
            OverlayView = null;
        }

        private void ShowDiceResultAnimation(object parameter)
        {
            if (!(parameter is int result))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            OverlayView = new DiceResultAnimationView(result);
        }

        private void ShowLoadingScreen()
        {
            Controls.LoadingScreen loadingScreen = new Controls.LoadingScreen();
            OverlayView = loadingScreen;
        }

        private void HideLoadingScreen()
        {
            OverlayView = null;
        }

        private void ShowFriendsView()
        {
            FriendsView friendsView = new Views.FriendsView();
            OverlayView = friendsView;

        }

        private async Task HideFriendsView()
        {
            if (OverlayView is Views.FriendsView friendsView && friendsView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }
            OverlayView = null; 
        }

        private void ShowFriendsRequestsView()
        {
            FriendRequestView friendsView = new Views.FriendRequestView();
            OverlayView = friendsView;

        }

        private async Task HideFriendsRequestsView()
        {
            if (OverlayView is Views.FriendRequestView friendsView && friendsView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
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
            InviteFriendView friendsView = new Views.InviteFriendView(accesKeyString);
            OverlayView = friendsView;

        }

        private async Task HideInviteFriendsView()
        {
            if (OverlayView is Views.InviteFriendView friendsView && friendsView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }
            OverlayView = null;
        }

        private void ShowScoreboardView()
        {
            ScoreboardView scoreboardView = new Views.ScoreboardView();
            OverlayView = scoreboardView;

        }

        private async Task HideScoreboardView()
        {
            if (OverlayView is Views.ScoreboardView scoreboardView && scoreboardView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDE_OUT_FROM_LEFT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }
            OverlayView = null;
        }

        private async Task ShowRegisterView()
        {
            if (currentView is Views.LoginView loginView && loginView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDE_OUT_FROM_TOP_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(1000);
            }

            CurrentView = new Views.RegisterView();
        }
        private async Task ShowLoginView()
        {
            if (currentView is Views.RegisterView registerView && registerView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)registerView.FindResource(Utilities.SLIDE_OUT_FROM_TOP_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(1000);
            }
            CurrentView = new Views.LoginView();
        }

        private async Task ShowNeedHelpView()
        {
            if (currentView is Views.LoginView loginView && loginView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)loginView.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(820);
            }
            CurrentView = new Views.NeedHelpView();
        }


        private async Task OcultVerifyAccountView()
        {
            if (currentView is Views.VerifyAccountView verifyAccountView && verifyAccountView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)verifyAccountView.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(820);
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
                if (registerView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDE_OUT_FROM_TOP_ANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }


            if (currentView is Views.LoginView loginview)
            {
                if (loginview.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)loginview.FindResource(Utilities.SLIDE_OUT_FROM_TOP_ANIMATION);
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

            if (currentView is Views.NeedHelpView needHelpView && needHelpView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)animatedGrid.FindResource(Utilities.SLIDE_OUT_FROM_TOP_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
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

            if (currentView is Views.CreateRoomView mainMenuView && mainMenuView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }

            GameLobbyView lobbyRoomView = new Views.GameLobbyView(gameDto);

            CurrentView = lobbyRoomView;
        }


        private void ShowGameScreen(object gameDtoObj)
        {
            if (!(gameDtoObj is ChatService.GameDto gameDto))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            CurrentView = new Gameplay.Views.GameFrameView(gameDto);
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
            ChangeMusic(Utilities.MAIN_MENU_MUSIC);
        }

        private async Task ShowCreateRoom()
        {
            if (currentView is Views.MainMenuView mainMenuView && mainMenuView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }
            CurrentView = new Views.CreateRoomView();
        }

        private async Task ShowLoginRoom()
        {
            if (currentView is Views.MainMenuView mainMenuView && mainMenuView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
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

            if (currentView is Views.MainMenuView mainMenuView && mainMenuView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)mainMenuView.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }

            ConfigureProfileView configureProfileView = new Views.ConfigureProfileView(accountDto);

            CurrentView = configureProfileView;
        }

        private async Task BackToMainMenu()
        {
            if (currentView is Views.CreateRoomView createRoomView)
            {
                if (createRoomView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
                {
                    Storyboard storyboard = (Storyboard)createRoomView.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }
            }
            else if (currentView is Views.LoginRoomView loginRoomView && loginRoomView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)loginRoomView.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }

            CurrentView = new Views.MainMenuView();
        }

        private async Task BackToGuestMainMenu()
        {
            if (currentView is Views.LoginRoomView loginRoomView && loginRoomView.FindName(Utilities.ANIMATED_GRID) is Grid animatedGrid)
            {
                Storyboard storyboard = (Storyboard)loginRoomView.FindResource(Utilities.SLIDE_OUT_FROM_RIGHT_ANIMATION);
                storyboard.Begin(animatedGrid);
                await Task.Delay(900);
            }

            CurrentView = new Views.GuestMainMenuView();
        }

    }
}
