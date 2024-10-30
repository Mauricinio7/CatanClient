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

namespace CatanClient.ViewModels
{
    internal class VerifyAccountChangeWindowViewModel : ViewModelBase
    {
        private string verificationCode;
        private AccountDto Account;


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
        private readonly ServiceManager serviceManager;

        public VerifyAccountChangeWindowViewModel(AccountDto account, ServiceManager serviceManager)
        {
            VerifyChangeCommand = new RelayCommand(ExecuteVerifyChange);
            this.serviceManager = serviceManager;
            Account = account;
        }

        private void ExecuteVerifyChange(object parameter)
        {
            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Account.TokenExpiration = DateTime.Now;
                Account.Token = VerificationCode;

                if (!String.IsNullOrEmpty(Account.Password))
                {
                    SendVerifyCodePassword();
                }
                else
                {
                    SendVerifyCodeEmailOrPhone();
                }
            }  
        }
        private void SendVerifyCodePassword()
        {
            OperationResultDto result;
            result = serviceManager.AccountServiceClient.ConfirmPassword(Account);
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

        private void SendVerifyCodeEmailOrPhone()
        {
            OperationResultChangeRegisterEmailOrPhone result;
            result = serviceManager.AccountServiceClient.ConfirmEmailOrPhone(Account);
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
    }
}
