using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CatanClient.ProfileService;
using CatanClient.UIHelpers;

namespace CatanClient.ViewModels
{
    internal class EditPasswordWindowViewModel : ViewModelBase
    {
        private string password;
        private string confirmPassword;
        private readonly ServiceManager serviceManager;
        public ICommand SaveCommand { get; }
        public ProfileDto Profile { get; set; }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        


        public EditPasswordWindowViewModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            AccountService.ProfileDto accountProfile = serviceManager.ProfileSingleton.Profile;
            accountProfile.PreferredLanguage = CultureInfo.CurrentCulture.Name;

            ProfileService.ProfileDto profileDto = AccountUtilities.CastAccountProfileToProfileService(accountProfile);

            Profile = profileDto;

            SaveCommand = new AsyncRelayCommand(OnSaveAsync);
        }

        private async Task OnSaveAsync()
        {
           if(AccountUtilities.VerificateChangePassword(Password, ConfirmPassword))
            {
                await SavePasswordAsync(Password);
            }
        }


        public async Task SavePasswordAsync(string password)
        {
            AccountService.AccountDto account = new AccountService.AccountDto
            {
                Email = string.Empty,
                PhoneNumber = string.Empty,
                Password = password,
                Id = Profile.Id,
                PreferredLanguage = CultureInfo.CurrentCulture.Name,
                CurrentSessionId = Profile.CurrentSessionID
            };
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            AccountService.OperationResultDto result = await serviceManager.AccountServiceClient.ChangePasswordAsync(account);

            if (result.IsSuccess)
            {
                ShowVerify(account);
            }
            else
            {
                MessageBox.Show(Utilities.MessageUnableToSaveData(CultureInfo.CurrentCulture.Name), Utilities.TitleUnableToSaveData(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        public static void ShowVerify(AccountService.AccountDto account)
        {
            VerifyAccountChangeWindow verifyWindow = new VerifyAccountChangeWindow(account);

            verifyWindow.ShowDialog();
        }
    }
}
