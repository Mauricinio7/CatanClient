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
        public ICommand SaveCommand { get; }

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
                if (AccountUtilities.VerificateChangePassword(Password, ConfirmPassword))
                {
                    bool resutl = await serviceManager.AccountServiceClient.ChangeForotPassword(Email, Password, VerificationCode);
                    if (resutl)
                    {
                        MessageBox.Show(Utilities.MessageSuccesPasswordChange(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
                        Mediator.Notify(Utilities.OCULT_VERIFY_ACCOUNT, null);
                    }
                    else
                    {
                        Utilities.ShowMessgeServerLost();
                    }
                }
            }
        }


    }    
}
