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

            Mediator.Notify("ShowVerifyAccountView");

            //bool isCreated = await ConnectToServerAsync();

            //if (isCreated)
            //{
            //   MessageBox.Show($"Bienvenido, {Username}!\nTu cuenta ha sido creada correctamente.", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
        }

        private async Task<bool> ConnectToServerAsync()
        {
            try
            {
                var binding = new NetTcpBinding("NetTcpBinding_IAccountEndPoint");
                var endpoint = new EndpointAddress("net.tcp://192.168.169.207:8081/AccountService");

                var channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
                var client = channelFactory.CreateChannel();

                var newAccount = new AccountDto
                {
                    Name = Username,
                    Email = email,
                    PhoneNumber = phoneNumber, 
                    Password = Password,
                    PicturePath = ""
                };

                await client.CreateAccountAsync(newAccount);

                ((IClientChannel)client).Close();
                channelFactory.Close();

                return true;
            }
            catch (FaultException ex)
            {
                MessageBox.Show($"Error del servicio: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (CommunicationException ex)
            {
                MessageBox.Show($"Error de comunicación: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show($"Error de tiempo de espera: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
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
