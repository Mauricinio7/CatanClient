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

namespace CatanClient.ViewModels
{
    internal class VerifyAccountViewModel : ViewModelBase
    {
        private AccountDto _account;
        private string _verificationCode;

        public AccountDto Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged(nameof(Account));
            }
        }

        public string VerificationCode
        {
            get => _verificationCode;
            set
            {
                _verificationCode = value;
                OnPropertyChanged(nameof(VerificationCode));
            }
        }
        public ICommand VerifyAccountCommand { get; }

        public VerifyAccountViewModel(AccountDto account)
        {
            VerifyAccountCommand = new RelayCommand(ExecuteVerifyAccount);
            Account = account;
        }

        private void ExecuteVerifyAccount(object parameter)
        {
            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                MessageBox.Show("El código de verificación está vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var status = VerifyUserAccount();

            if (status)
            {

               MessageBox.Show("Usuario verificado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
               Mediator.Notify("OcultVerifyAccountView", null);
            }
            else
            {
                MessageBox.Show("No se ha podido verificar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //TODO is a service
        public bool VerifyUserAccount()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://192.168.1.127:8181/AccountService"); //TODO quit harcode
            var channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result;

            try
            {
              Account.Token = VerificationCode;

                MessageBox.Show(Account.Token);
                MessageBox.Show(Account.Email);

                result = client.VerifyAccountAsync(Account).Result;

                MessageBox.Show(result.IsSuccess + " " + result.MessageResponse);

                return result.IsSuccess;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }

        }
    }
}
