using CatanClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CatanClient.Views;
using System.Windows.Media.Animation;
using CatanClient.UIHelpers;
using System.Linq.Expressions;
using CatanClient.AccountService;
using System.ServiceModel;
using System.Security.Principal;
using System.Globalization;
using Serilog;
using System.Text.RegularExpressions;
using CatanClient.Services;
using CatanClient.Singleton;
using CatanClient.GuestAccountService;

namespace CatanClient.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private string username;
        private string password;
        private string email;
        private string phoneNumber;
        private readonly ServiceManager serviceManager;
        public ICommand LoginCommand { get; }
        public ICommand JoinGuestCommand { get; }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }


        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Email
        {             get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        
        

        public LoginViewModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            LoginCommand = new AsyncRelayCommand(ExecuteLoginAsync);
            JoinGuestCommand = new AsyncRelayCommand(ExecuteJoinGuestAsync);
        }

        private async Task ExecuteJoinGuestAsync(object window)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
            var result = await serviceManager.GuestAccountServiceClient.LoginAsGuestAsync(CultureInfo.CurrentCulture.Name);

            if (result.IsSuccess)
            {
                var guestProfile = new ProfileDto
                {
                    Id = result.GuestAccount.Id,
                    Name = result.GuestAccount.Name,
                    PreferredLanguage = CultureInfo.CurrentCulture.Name,
                    IsRegistered = false
                };

                serviceManager.ProfileSingleton.SetProfile(guestProfile);
                ShowMainMenu(window, true);
            }
            else
            {
                MessageBox.Show(Utilities.MessageServerLostConnection(CultureInfo.CurrentCulture.Name), Utilities.TittleServerLostConnection(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private async Task ExecuteLoginAsync(object actualWindow)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Email = AccountUtilities.IsValidAccountEmail(Username) ? Username : string.Empty;
                PhoneNumber = AccountUtilities.IsValidAccountPhoneNumber(Username) ? Username : string.Empty;

                if ((!AccountUtilities.IsValidAccountEmail(Email) && !AccountUtilities.IsValidAccountPhoneNumber(PhoneNumber)) || !AccountUtilities.IsValidAccountPassword(Password))
                {
                    MessageBox.Show(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name), Utilities.TittleInvalidCaracters(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    AccountDto account = AccountUtilities.CreateAccount(Email, PhoneNumber, Password, string.Empty);
                    await AuthenticateUserAsync(account, actualWindow);
                }
            }
        }

        internal async Task AuthenticateUserAsync(AccountDto account, object window)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
            OperationResultProfileDto result = await serviceManager.AccountServiceClient.IsValidAuthenticationAsync(account);
            switch (result.AunthenticationStatus)
            {
                case EnumAuthenticationStatus.Verified:
                    serviceManager.ProfileSingleton.SetProfile(result.ProfileDto);
                    ShowMainMenu(window, false);
                    break;
                case EnumAuthenticationStatus.NotVerified:
                    //TODO get Account id
                    ShowVerifyAccountView(account);
                    break;
                case EnumAuthenticationStatus.Incorrect:
                    MessageBox.Show(Utilities.MessageIncorrectPasswordOrUsername(CultureInfo.CurrentCulture.Name), Utilities.TittleIncorrectPasswordOrUsername(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case EnumAuthenticationStatus.ServerNotFound:
                    MessageBox.Show(Utilities.MessageServerLostConnection(CultureInfo.CurrentCulture.Name), Utilities.TittleServerLostConnection(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

        internal void ShowMainMenu(object actualWindow, bool isGuest)
        {
            MessageBox.Show(Utilities.DialogWelcome(CultureInfo.CurrentCulture.Name) + ": " + serviceManager.ProfileSingleton.Profile.Name, Utilities.DialogWelcome(CultureInfo.CurrentCulture.Name) + "!", MessageBoxButton.OK, MessageBoxImage.Information);

                if (actualWindow is Window ventanaActual)
                {
                    ventanaActual.IsEnabled = false;
                    Storyboard fadeOutStoryboard = ventanaActual.FindResource(Utilities.FADE_OUT_ANIMATION) as Storyboard;
                    if (fadeOutStoryboard != null)
                    {
                        fadeOutStoryboard.Completed += (s, e) =>
                        {
                            Mediator.Notify(Utilities.SHOW_MAIN_MENU_BACKGROUND, null);

                            if (isGuest)
                            {
                                Mediator.Notify(Utilities.SHOW_GUEST_MAIN_MENU, null);
                            }
                            else
                            {
                                Mediator.Notify(Utilities.SHOW_MAIN_MENU, null);
                            }
                            
                            Storyboard fadeInStoryboard = ventanaActual.FindResource(Utilities.FADE_IN_ANIMATION) as Storyboard;
                            if (fadeInStoryboard != null)
                            {
                                fadeInStoryboard.Completed += (sender, args) =>
                                {
                                    ventanaActual.IsEnabled = true;
                                };
                                fadeInStoryboard.Begin(ventanaActual);
                                ventanaActual.IsEnabled = true;
                            }
                        };

                        fadeOutStoryboard.Begin(ventanaActual);
                    }
                }
        }

        internal static void ShowVerifyAccountView(AccountDto account)
        {
            MessageBox.Show(Utilities.MessageUnverifiedUser(CultureInfo.CurrentCulture.Name), Utilities.TittleUnverifiedUser(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            Mediator.Notify(Utilities.SHOW_VERIFY_ACCOUNT, account);
        }

    }
}
