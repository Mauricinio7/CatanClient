using CatanClient.AccountService;
using CatanClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Security.Principal;
using System.Xml.Linq;
using CatanClient.UIHelpers;
using System.Globalization;
using Serilog;
using CatanClient.Services;

namespace CatanClient.ViewModels
{
    internal class VerifyAccountViewModel : ViewModelBase
    {
        private AccountDto account;
        private string verificationCode;

        public AccountDto Account
        {
            get => account;
            set
            {
                account = value;
                OnPropertyChanged(nameof(Account));
            }
        }

        public string VerificationCode
        {
            get => verificationCode;
            set
            {
                verificationCode = value;
                OnPropertyChanged(nameof(VerificationCode));
            }
        }
        public ICommand VerifyAccountCommand { get; }
        public ICommand ResentCodeCommand { get; }
        private readonly ServiceManager serviceManager;

        public VerifyAccountViewModel(AccountDto account, ServiceManager serviceManager)
        {
            VerifyAccountCommand = new AsyncRelayCommand(ExecuteVerifyAccountAsync);
            ResentCodeCommand = new AsyncRelayCommand(ExecuteResendCodeAsync);
            Account = account;
            this.serviceManager = serviceManager;
        }

        private async Task ExecuteVerifyAccountAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
            bool status = await serviceManager.AccountServiceClient.VerifyUserAccountAsync(Account, VerificationCode);

            if (status)
            {
                MessageBox.Show(Utilities.MessageSuccessVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
                Mediator.Notify(Utilities.OCULTVERIFYACCOUNT, null);
            }
            else
            {
                MessageBox.Show(Utilities.MessageFailVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task ExecuteResendCodeAsync(object parameter)
        {
            bool result = await serviceManager.AccountServiceClient.ResendCodeAsync(Account);

            if (result)
            {
                MessageBox.Show(Utilities.MessageSuccesSendVerificationCode(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }


    }
}
