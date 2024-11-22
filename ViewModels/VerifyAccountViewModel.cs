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

        private async Task ExecuteVerifyAccountAsync()
        {
            try
            {
                AccountUtilities.ValidateVerificationCode(VerificationCode);

                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

                bool result = await AccountUtilities.VerifyAccountAsync(serviceManager, account, VerificationCode);

                if (result)
                {
                    Mediator.Notify(Utilities.OCULT_VERIFY_ACCOUNT, null);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task ExecuteResendCodeAsync()
        {
            await AccountUtilities.ResendVerificationCodeAsync(serviceManager, account);
        }


    }
}
