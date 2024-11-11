using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CatanClient.AccountService;
using System.Security.Principal;

namespace CatanClient.ViewModels
{
    internal class ChangeForgotPasswordViewModel : ViewModelBase
    {
        private string Email;
        private string password;
        private string confirmPassword;
        private string verificationCode;

        public string VerificationCode
        {
            get => verificationCode;
            set
            {
                verificationCode = value;
                OnPropertyChanged(nameof(VerificationCode));
            }
        }

        private readonly ServiceManager serviceManager;

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }


        public ICommand SaveCommand { get; }


        public ChangeForgotPasswordViewModel(string email, ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;

            Email = email;

            SaveCommand = new AsyncRelayCommand(OnSaveAsync);
        }



        private async Task OnSaveAsync()
        {
            if (string.IsNullOrEmpty(VerificationCode))
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    if (AccountUtilities.IsValidAccountPassword(Password))
                    {
                        if (Password == ConfirmPassword)
                        { 
                            bool resutl = await serviceManager.AccountServiceClient.ChangeForotPassword(Email, Password, VerificationCode);
                            if (resutl)
                            {
                                MessageBox.Show(Utilities.MessageSuccesPasswordChange(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
                                Mediator.Notify(Utilities.OCULTVERIFYACCOUNT, null);
                            }
                            else
                            {
                                Utilities.ShowMessgeServerLost();
                            }
                        }
                        else
                        {
                            MessageBox.Show(Utilities.MessagePasswordNotMacth(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name), Utilities.TittleInvalidCaracters(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


    }    
}
