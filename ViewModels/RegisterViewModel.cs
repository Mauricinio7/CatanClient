using CatanClient.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CatanClient.AccountService;
using System.ServiceModel;
using System.Text.RegularExpressions;
using CatanClient.UIHelpers;
using System.Globalization;
using CatanClient.Services;

namespace CatanClient.ViewModels
{
    internal class RegisterViewModel : ViewModelBase
    {
        private string contactInfo;
        private string password;
        private string username;

        private string email;
        private string phoneNumber;

        public string ContactInfo
        {
            get { return contactInfo; }
            set
            {
                contactInfo = value;
                OnPropertyChanged(nameof(ContactInfo));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public ICommand RegisterCommand { get; set; }
        private readonly ServiceManager serviceManager;


        public RegisterViewModel(ServiceManager serviceManager)
        {
            RegisterCommand = new RelayCommand(async () => await RegisterUserAsync());
            this.serviceManager = serviceManager;
        }

        private async Task RegisterUserAsync()
        {
            if (IsNotNullOrEmptyAccound(Username, ContactInfo, Password))
            {
                if (AccountUtilities.IsValidAccountName(Username) && AccountUtilities.IsValidAccountPassword(Password) &&
                    (AccountUtilities.IsValidAccountEmail(ContactInfo) || AccountUtilities.IsValidAccountPhoneNumber(ContactInfo)))
                {
                    email = AccountUtilities.IsValidAccountEmail(ContactInfo) ? ContactInfo : String.Empty;
                    phoneNumber = AccountUtilities.IsValidAccountPhoneNumber(ContactInfo) ? ContactInfo : String.Empty;

                    AccountDto account = AccountUtilities.CreateAccount(email, phoneNumber, Password, Username);
                    
                    bool isCreated = await serviceManager.AccountServiceClient.CreateAccountInServerAsync(account); 

                    if (isCreated)
                    {
                        Mediator.Notify(Utilities.SHOWVERIFYACCOUNT, account);
                    }
                }
                else
                {
                    MessageBox.Show(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name), Utilities.TittleInvalidCaracters(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        

        public bool IsNotNullOrEmptyAccound(string name, string ContactInfo, string password)
        {
            return (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(ContactInfo) && !string.IsNullOrEmpty(password));
        }

       

    }
}
