using CatanClient.Commands;
using CatanClient.ProfileService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class AddFriendWindowViewModel : ViewModelBase
    {
        public ICommand AddFriendCommand { get; }
        private string playerName;
        private readonly ServiceManager serviceManager;
        private readonly ProfileDto profile;

        public string PlayerName
        {
            get => playerName;
            set
            {
                if (playerName != value)
                {
                    playerName = value;
                    OnPropertyChanged(nameof(PlayerName));
                }
            }
        }

        public AddFriendWindowViewModel(ServiceManager serviceManager)
        {
            AddFriendCommand = new RelayCommand(ExecuteAddFriend);
            this.serviceManager = serviceManager;
            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profile = AccountUtilities.CastAccountProfileToProfileService(profileDto);
        }

        internal void ExecuteAddFriend(object parameter)
        {
            if (string.IsNullOrEmpty(PlayerName))
            {
                Utilities.ShowMessageEmptyFields();
            }
            else if(!AccountUtilities.IsValidLength(PlayerName)) 
            {
                Utilities.ShowMessageInvalidFileds();
            }
            else
            {
                PlayerName = PlayerName.Trim();
                bool result;
                result = serviceManager.ProfileServiceClient.SendFriendRequest(PlayerName, profile);

                if (result)
                {
                    MessageBox.Show(Utilities.MessageSuccesFriendRequest(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //TODO Friend not Found
                    Utilities.ShowMessgeServerLost();
                }
            }


        }

    }
}
