using CatanClient.AccountService;
using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Security.Principal;

namespace CatanClient.ViewModels
{
    internal class VerifyAccountChangeWindowViewModel : ViewModelBase
    {
        private string verificationCode;
        private readonly AccountDto Account;


        public string VerificationCode
        {
            get => verificationCode;
            set
            {
                verificationCode = value;
                OnPropertyChanged(nameof(VerificationCode));
            }
        }
        public ICommand VerifyChangeCommand { get; }
        public ICommand ResentCodeCommand { get; }
        private readonly ServiceManager serviceManager;

        public VerifyAccountChangeWindowViewModel(AccountDto account, ServiceManager serviceManager)
        {
            VerifyChangeCommand = new AsyncRelayCommand(ExecuteVerifyChangeAsync);
            ResentCodeCommand = new AsyncRelayCommand(ExecuteResendCodeAsync);
            this.serviceManager = serviceManager;
            Account = account;
        }

        private async Task ExecuteVerifyChangeAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Account.TokenExpiration = DateTime.Now;
                Account.Token = VerificationCode;
                

                if (!string.IsNullOrEmpty(Account.Password))
                {
                    await SendVerifyCodePasswordAsync(); 
                }
                else
                {
                    await SendVerifyCodeEmailOrPhoneAsync(); 
                }
            }
        }
        private async Task SendVerifyCodePasswordAsync()
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
            var result = await serviceManager.AccountServiceClient.ConfirmPasswordAsync(Account); 

            if (result.IsSuccess)
            {
                MessageBox.Show(Utilities.MessageSuccessVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
                AccountUtilities.RestartGame();
            }
            else
            {
                MessageBox.Show(Utilities.MessageFailVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task SendVerifyCodeEmailOrPhoneAsync()
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
            var result = await serviceManager.AccountServiceClient.ConfirmEmailOrPhoneAsync(Account); 

            if (result.IsSuccess)
            {
                MessageBox.Show(Utilities.MessageSuccessVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
                AccountUtilities.RestartGame();
            }
            else
            {
                MessageBox.Show(Utilities.MessageFailVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task ExecuteResendCodeAsync()
        {
            await AccountUtilities.ResendVerificationCodeAsync(serviceManager, Account);
        }
    }
}
