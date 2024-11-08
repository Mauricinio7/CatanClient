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
        private ProfileDto profile;
        private readonly ServiceManager serviceManager;

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

        public ProfileDto Profile { get => profile; set => profile = value; }

        public ICommand SaveCommand { get; }


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
            if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                if (AccountUtilities.IsValidAccountPassword(Password))
                {
                    if (Password == ConfirmPassword)
                    {
                        await SavePasswordAsync(Password); 
                    }
                    else
                    {
                        MessageBox.Show(Utilities.MessagePasswordNotMacth(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name), Utilities.TittleInvalidCaracters(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
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


        public void ShowVerify(AccountService.AccountDto account)
        {
            var verifyWindow = new VerifyAccountChangeWindow(account);

            verifyWindow.ShowDialog();
        }
    }
}
