using CatanClient.AccountService;
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

namespace CatanClient.ViewModels
{
    internal class ConfigureProfileViewModel : ViewModelBase
    {
        public ICommand ModifyProfileCommand { get; }
        

        private string _username;
        private AccountDto account;
        private string _email;
        private string _phone;
        private ProfileDto profile;


        public AccountDto Account
        {
            get => account;
            set
            {
                account = value;
                OnPropertyChanged(nameof(Account));
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));  
            }
        }

        public string Email
        
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));  
            }
        }


        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));  
            }
        }

        public ProfileDto Profile { get => profile; set => profile = value; }

        private readonly ServiceManager serviceManager;
        public ConfigureProfileViewModel(AccountDto account, ServiceManager serviceManager)
        {

            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profileDto.PreferredLanguage = CultureInfo.CurrentCulture.Name;

            Profile = profileDto;

            ModifyProfileCommand = new RelayCommand(OnModifyProfile);
            this.serviceManager = serviceManager;

            Account = account;

            Username = Profile.Name;
            Email = account.Email;
            Phone = account.PhoneNumber;
        }

        private void OnModifyProfile(object parameter)
        {
            if (parameter is string field)
            {
                var editWindow = new EditProfileWindow(field);

                editWindow.ShowDialog();
            }
        }
    }
}
