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
        public string Field { get; }

        public VerifyAccountChangeWindowViewModel(AccountDto account, ServiceManager serviceManager)
        {
            VerifyChangeCommand = new RelayCommand(ExecuteVerifyChange);
            this.serviceManager = serviceManager;
            Field = "Email";
            Account = account;
        }

        private void ExecuteVerifyChange(object parameter)
        {
            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Field == "Email")
            {
                MessageBox.Show("Email");

                Account.TokenExpiration = DateTime.Now;
                Account.Token = VerificationCode;

                OperationResultChangeRegisterEmailOrPhone result;

                result = serviceManager.AccountServiceClient.ConfirmEmail(Account);

                if (result.IsSuccess)
                {
                    MessageBox.Show(Utilities.MessageSuccessVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(Utilities.MessageFailVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Phone");
                //bool status = serviceManager.AccountServiceClient.VerifyPhoneChange(VerificationCode);
                if (true)//status)
                {
                    MessageBox.Show(Utilities.MessageSuccessVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(Utilities.MessageFailVerifyUser(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }
    }
}
