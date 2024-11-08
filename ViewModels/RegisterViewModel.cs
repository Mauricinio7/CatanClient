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
using System.IO;
using CatanClient.Properties;

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

                    Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

                    bool isCreated = await serviceManager.AccountServiceClient.CreateAccountInServerAsync(account); 

                    if (isCreated)
                    {
                        string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "ShibaTest.png");
                        SaveImageInServer(filePath, account);

                        Mediator.Notify(Utilities.SHOWVERIFYACCOUNT, account);
                    }
                    else
                    {
                        Utilities.ShowMessgeServerLost();
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

        private void SaveImageInServer(string filePath, AccountDto account)
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);

            ProfileService.OperationResultProfileDto result;

            ProfileService.ProfileDto profileDto = new ProfileService.ProfileDto
            {
                Id = account.Id,
                PreferredLanguage = CultureInfo.CurrentCulture.Name,
                Name = account.Name
            };

            result = serviceManager.ProfileServiceClient.UploadImage(profileDto, imageBytes);

            if (result.IsSuccess)
            {
                SaveImageLocally(filePath, account);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }

        }

        private void SaveImageLocally(string filePath, AccountDto account)
        {
            string appDirectory = Path.Combine(Environment.CurrentDirectory, Utilities.PROFILE_IMAGE_DIRECTORY);

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            string fileName = Utilities.ProfilePhotoPath(account.Id.Value);
            string destinationPath = Path.Combine(appDirectory, fileName);

            File.Copy(filePath, destinationPath, overwrite: true);
        }



        public bool IsNotNullOrEmptyAccound(string name, string ContactInfo, string password)
        {
            return (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(ContactInfo) && !string.IsNullOrEmpty(password));
        }

       

    }
}
