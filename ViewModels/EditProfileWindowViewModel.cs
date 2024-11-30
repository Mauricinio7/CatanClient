using CatanClient.ProfileService;
using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class EditProfileWindowViewModel : ViewModelBase
    {

        private string newValue;
        private readonly ServiceManager serviceManager;
        public ICommand SaveCommand { get; }
        public string Field { get; }
        public ProfileDto Profile { get; set; }
        public string PromptText => SetPrompt();
        public string NewValue
        {
            get => newValue;
            set
            {
                newValue = value;
                OnPropertyChanged(nameof(NewValue));
            }
        }

        

        public EditProfileWindowViewModel(string field, ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            Field = field;
            AccountService.ProfileDto accountProfile = serviceManager.ProfileSingleton.Profile;
            accountProfile.PreferredLanguage = CultureInfo.CurrentCulture.Name;

            ProfileService.ProfileDto profileDto = AccountUtilities.CastAccountProfileToProfileService(accountProfile);         

            Profile = profileDto;

            SaveCommand = new AsyncRelayCommand(OnSaveAsync);
        }

        private string SetPrompt()
        {
            string prompt = String.Empty;

            switch (Field)
            {
               case Utilities.USERNAME:
                    prompt = Utilities.EnterNew(CultureInfo.CurrentCulture.Name) + " " + Utilities.Username(CultureInfo.CurrentCulture.Name) + ":";
                    break;
                case Utilities.EMAIL:
                    prompt = Utilities.EnterNew(CultureInfo.CurrentCulture.Name) + " " + Utilities.Email(CultureInfo.CurrentCulture.Name) + ":";
                    break;
                case Utilities.PHONE:
                    prompt = Utilities.EnterNew(CultureInfo.CurrentCulture.Name) + " " + Utilities.PhoneNumber(CultureInfo.CurrentCulture.Name) + ":";
                    break;
            }
            return prompt;

        }


        private async Task OnSaveAsync()
        {
            if (!string.IsNullOrEmpty(NewValue))
            {
                if (Field == Utilities.USERNAME)
                {
                    SaveUsername(NewValue);
                }
                else
                {
                    await SaveEmailOrPhoneAsync(NewValue, Field);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void SaveUsername(string username)
        {
            if (AccountUtilities.IsValidAccountName(username))
            {
                OperationResultProfileDto result = serviceManager.ProfileServiceClient.ChangeName(Profile, username);
                if (result.IsSuccess)
                {
                    MessageBox.Show(Utilities.MessageChangeUsernameSucces(CultureInfo.CurrentCulture.Name), Utilities.TitleChangeUsername(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    serviceManager.ProfileSingleton.SetName(username);

                    Mediator.Notify(Utilities.CLOSE_EDIT_PROFILE, null);
                    Mediator.Notify(Utilities.BACK_TO_MAIN_MENU_ROOM, null);
                }
                else
                {
                    MessageBox.Show(Utilities.MessageChangeUsernameFail(CultureInfo.CurrentCulture.Name), Utilities.TitleChangeUsername(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name), Utilities.TittleInvalidCaracters(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async Task SaveEmailOrPhoneAsync(string newValue, string type)
        {
            if (AccountUtilities.IsValidAccountEmail(newValue) || AccountUtilities.IsValidAccountPhoneNumber(newValue))
            {
                AccountService.AccountDto account = new AccountService.AccountDto
                {
                    Email = string.Empty,
                    PhoneNumber = string.Empty,
                    Password = string.Empty,
                    Id = Profile.Id,
                    PreferredLanguage = CultureInfo.CurrentCulture.Name,
                    CurrentSessionId = Profile.CurrentSessionID
                };

                if (type == Utilities.EMAIL)
                {
                    account.Email = newValue;
                }
                else
                {
                    account.PhoneNumber = newValue;
                }

                AccountService.OperationResultChangeRegisterEmailOrPhone result = await serviceManager.AccountServiceClient.ChangeEmailOrPhoneAsync(account);

                if (result.IsSuccess)
                {
                    ShowVerify(account);
                }
                else
                {
                    MessageBox.Show(Utilities.MessageUnableToSaveData(CultureInfo.CurrentCulture.Name), Utilities.TitleUnableToSaveData(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name), Utilities.TittleInvalidCaracters(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        public static void ShowVerify(AccountService.AccountDto account)
        {
            VerifyAccountChangeWindow verifyWindow = new VerifyAccountChangeWindow(account);
                
            verifyWindow.ShowDialog();
        }

        



    }
}
