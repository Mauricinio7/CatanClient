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

namespace CatanClient.ViewModels
{
    internal class RegisterViewModel : ViewModelBase
    {
        private string _contactInfo;
        private string _password;
        private string _username;
        private AccountDto account;

        private string email;
        private string phoneNumber;

        public string ContactInfo
        {
            get { return _contactInfo; }
            set
            {
                _contactInfo = value;
                OnPropertyChanged(nameof(ContactInfo));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public ICommand RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(async () => await RegisterUserAsync());
        }

        private async Task RegisterUserAsync()
        {
            if (IsNotNullOrEmptyAccound(Username, ContactInfo, Password))
            {
                if (IsValidAccountName(Username) && IsValidAccountPassword(Password) &&
                    (IsValidAccountEmail(ContactInfo) || IsValidAccountPhoneNumber(ContactInfo)))
                {
                    email = IsValidAccountEmail(ContactInfo) ? ContactInfo : "";
                    phoneNumber = IsValidAccountPhoneNumber(ContactInfo) ? ContactInfo : "";
                    
                    bool isCreated = await ConnectToServerAsync();

                    if (isCreated)
                    {
                        Mediator.Notify("ShowVerifyAccountView", account);
                    }
                }
                else
                {
                    MessageBox.Show("Se han introducido datos inválidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Se han dejado campos vacíos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            

            
        }

        //TODO is a service
        private async Task<bool> ConnectToServerAsync()
        {

            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://192.168.1.127:8181/AccountService"); //TODO quit harcode
            var channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result;

            try
            {
                account = new AccountDto
                {
                    Name = Username,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Password = Password,
                    PicturePath = String.Empty,
                    PreferredLanguage = "en"
                };

                result = await client.CreateAccountAsync(account);
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

        public bool IsNotNullOrEmptyAccound(string name, string ContactInfo, string password)
        {
            return (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(ContactInfo) && !string.IsNullOrEmpty(password));
        }

        public bool IsValidAccountName(string name)
        {
            string nameRegex = "^[a-zA-Z0-9 ]+$";

            return Regex.IsMatch(name, nameRegex);
        }

        public bool IsValidAccountEmail(string email)
        {
            string emailRegex = @"^[a-zA-Z0-9._-]+(?<!\.)@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(email, emailRegex)) return false;

            string[] parts = email.Split('@');

            if (parts.Length != 2) return false;

            string localPart = parts[0];
            string domainPart = parts[1];

            if (Regex.IsMatch(localPart, "\\.\\.+") || Regex.IsMatch(domainPart, "\\.\\.+")) return false;

            if (localPart.Contains("..")) return false;

            if (domainPart.StartsWith("-") || domainPart.EndsWith("-") ||
                domainPart.StartsWith(".") || domainPart.EndsWith(".")) return false;

            return true;
        }


        public bool IsValidAccountPhoneNumber(string phoneNumber)
        {
            string phoneNumberRegex = "^[0-9]+$";

            return Regex.IsMatch((string)phoneNumber, phoneNumberRegex);
        }

        public bool IsValidAccountPassword(string password)
        {
            string passwordRegex = "^[a-zA-Z0-9_]+$";
            return Regex.IsMatch(password, passwordRegex);
        }

    }
}
