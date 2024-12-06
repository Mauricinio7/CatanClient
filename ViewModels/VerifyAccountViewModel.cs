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
                if (string.IsNullOrEmpty(VerificationCode))
                {
                    Utilities.ShowMessageEmptyFields();
                    return;
                }
                else if (AccountUtilities.IsValidLength(VerificationCode))
                {
                    MessageBox.Show(Utilities.MessageTooLargeInput(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

                    bool result = await AccountUtilities.VerifyAccountAsync(serviceManager, account, VerificationCode);

                    if (result)
                    {
                        Mediator.Notify(Utilities.OCULT_VERIFY_ACCOUNT, null);
                    }
                }
                  
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
            }
        }

        private async Task ExecuteResendCodeAsync()
        {
            await AccountUtilities.ResendVerificationCodeAsync(serviceManager, account);
        }


    }
}
