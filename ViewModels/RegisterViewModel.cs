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
using System.Linq.Expressions;

namespace CatanClient.ViewModels
{
    internal class RegisterViewModel : ViewModelBase
    {
        private string contactInfo;
        private string password;
        private string username;

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
            ContactInfo = ContactInfo.Trim();
            string email = string.Empty;
            string phoneNumber = string.Empty;

            if (IsNotNullOrEmptyAccound(Username, ContactInfo, Password))
            {
                if (AccountUtilities.IsValidAccountName(Username) && AccountUtilities.IsValidAccountPassword(Password) &&
                    (AccountUtilities.IsValidAccountEmail(ContactInfo) || AccountUtilities.IsValidAccountPhoneNumber(ContactInfo)))
                {
                    email = AccountUtilities.IsValidAccountEmail(ContactInfo) ? ContactInfo : String.Empty;
                    phoneNumber = AccountUtilities.IsValidAccountPhoneNumber(ContactInfo) ? ContactInfo : String.Empty;

                    AccountDto account = AccountUtilities.CreateAccount(email, phoneNumber, Password, Username);

                    Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

                    OperationResultCreateAccountDto result = await serviceManager.AccountServiceClient.CreateAccountInServerAsync(account);

                    HandledRegisterUser(result, account);

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

        internal void HandledRegisterUser(OperationResultCreateAccountDto result, AccountDto account)
        {
            switch (result.status)
            {
                case EnumCreateAccountStatus.SuccessSave:
                    ProfileDto profile = result.ProfileDto;
                    string packUri = Utilities.DEFAULT_PHOTO_PATH;
                    SaveImageInServer(packUri, profile);

                    account.Id = profile.Id;

                    Mediator.Notify(Utilities.SHOW_VERIFY_ACCOUNT, account);
                    break;
                case EnumCreateAccountStatus.ExistsAccount:
                    MessageBox.Show(Utilities.MessageAccountInUse(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case EnumCreateAccountStatus.ExistsName:
                    MessageBox.Show(Utilities.MessageNameInUse(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case EnumCreateAccountStatus.ErrorSaving:
                    Utilities.ShowMessgeServerLost();
                    break;
            }
        }


        private void SaveImageInServer(string packUri, ProfileDto profile)
        {
            byte[] imageBytes = ReadImageFromPackUri(packUri);

            ProfileService.OperationResultProfileDto result;

            profile.IsOnline = true;

            result = serviceManager.ProfileServiceClient.UploadImage(AccountUtilities.CastAccountProfileToProfileService(profile), imageBytes);

            if (result.IsSuccess)
            {
                SaveImageLocally(packUri, profile);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        private static byte[] ReadImageFromPackUri(string packUri)
        {
            Uri uri = new Uri(packUri, UriKind.Absolute);

            using (Stream resourceStream = Application.GetResourceStream(uri).Stream)
            using (MemoryStream memoryStream = new MemoryStream())
            {
                resourceStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static void SaveImageLocally(string packUri, ProfileDto profile)
        {
            string appDirectory = Path.Combine(Environment.CurrentDirectory, Utilities.PROFILE_IMAGE_DIRECTORY);

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            string fileName = Utilities.ProfilePhotoPath(profile.Id.Value);
            string destinationPath = Path.Combine(appDirectory, fileName);

            byte[] imageBytes = ReadImageFromPackUri(packUri);
            File.WriteAllBytes(destinationPath, imageBytes);
        }



        public static bool IsNotNullOrEmptyAccound(string name, string ContactInfo, string password)
        {
            return (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(ContactInfo) && !string.IsNullOrEmpty(password));
        }

       

    }
}
