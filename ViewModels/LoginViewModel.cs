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

        public ICommand LoginCommand { get; }
        public ICommand JoinGuestCommand { get; }
        

        public LoginViewModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            LoginCommand = new RelayCommand(ExecuteLogin);
            JoinGuestCommand = new RelayCommand(ExecuteJoinGuest);
        }

        private void ExecuteJoinGuest(object window)
        {
            OperationResultGuestAccountDto result;
            result = serviceManager.GuestAccountServiceClient.LoginAsGuest(CultureInfo.CurrentCulture.Name);

            if(result.IsSuccess)
            {
                ProfileDto guestProfile = new ProfileDto
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

        private void ExecuteLogin(object actualWindow) 
        {

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Email = AccountUtilities.IsValidAccountEmail(Username) ? Username : String.Empty;
                PhoneNumber = AccountUtilities.IsValidAccountPhoneNumber(Username) ? Username : String.Empty;

                if ((!AccountUtilities.IsValidAccountEmail(Email) && !AccountUtilities.IsValidAccountPhoneNumber(PhoneNumber)) || !AccountUtilities.IsValidAccountPassword(Password))
                {
                    MessageBox.Show(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name), Utilities.TittleInvalidCaracters(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    AccountDto account = AccountUtilities.CreateAccount(Email, PhoneNumber, Password, String.Empty);

                    AuthenticateUser(account, actualWindow);

                    //TODO just for Test WARING
                    //ShowMainMenu(actualWindow, false);

                }
            } 
            
        }

        internal void AuthenticateUser(AccountDto account, object window)
        {
            OperationResultProfileDto result = serviceManager.AccountServiceClient.IsValidAuthentication(account);


            switch (result.AunthenticationStatus)
            {
                case AccountService.EnumAuthenticationStatus.Verified:

                    serviceManager.ProfileSingleton.SetProfile(result.ProfileDto);

                    MessageBox.Show(result.ProfileDto.CurrentSessionID);

                    ShowMainMenu(window, false);
                    break;
                case AccountService.EnumAuthenticationStatus.InGame: //TODO: Send to room

                    serviceManager.ProfileSingleton.SetProfile(result.ProfileDto);

                    ShowMainMenu(window, false);
                    break;
                case AccountService.EnumAuthenticationStatus.NotVerified:
                    ShowVerifyAccountView(account);
                    break;
                case AccountService.EnumAuthenticationStatus.Incorrect:
                    MessageBox.Show(Utilities.MessageIncorrectPasswordOrUsername(CultureInfo.CurrentCulture.Name), Utilities.TittleIncorrectPasswordOrUsername(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case AccountService.EnumAuthenticationStatus.ServerNotFound:
                    MessageBox.Show(Utilities.MessageServerLostConnection(CultureInfo.CurrentCulture.Name), Utilities.TittleServerLostConnection(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

        internal void ShowMainMenu(object actualWindow, bool isGuest)
        {
            MessageBoxResult result = MessageBox.Show(Utilities.DialogWelcome(CultureInfo.CurrentCulture.Name) + ":" + Username, Utilities.DialogWelcome(CultureInfo.CurrentCulture.Name) + "!", MessageBoxButton.OK, MessageBoxImage.Information);

                if (actualWindow is Window ventanaActual)
                {
                    ventanaActual.IsEnabled = false;
                    Storyboard fadeOutStoryboard = ventanaActual.FindResource(Utilities.FADEOUTANIMATION) as Storyboard;
                    if (fadeOutStoryboard != null)
                    {
                        fadeOutStoryboard.Completed += (s, e) =>
                        {
                            Mediator.Notify(Utilities.SHOWMAINMENUBACKGROUND, null);

                            if (isGuest)
                            {
                                Mediator.Notify(Utilities.SHOW_GUEST_MAIN_MENU, null);
                            }
                            else
                            {
                                Mediator.Notify(Utilities.SHOWMAINMENU, null);
                            }
                            
                            Storyboard fadeInStoryboard = ventanaActual.FindResource(Utilities.FADEINANIMATION) as Storyboard;
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

        internal void ShowVerifyAccountView(AccountDto account)
        {
            MessageBox.Show(Utilities.MessageUnverifiedUser(CultureInfo.CurrentCulture.Name), Utilities.TittleUnverifiedUser(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            Mediator.Notify(Utilities.SHOWVERIFYACCOUNT, account);
        }

    }
}
