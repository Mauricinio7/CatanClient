using CatanClient.Commands;
using CatanClient.ProfileService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CatanClient.Controls
{
    internal class InvitePlayerCardViewModel : ViewModelBase
    {

        private ServiceManager serviceManager;
        private BitmapImage imageSource;
        public string PlayerName { get; set; }
        public string AccesKey { get; set; }
        public bool IsOnline { get; set; }
        public ICommand InviteCommand { get; }
        public ProfileDto SenderProfile { get; set; }

        private ProfileDto Profile { get; set; }

        public BitmapImage ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public InvitePlayerCardViewModel(ProfileDto profile, string accesKey, ServiceManager serviceManager)
        {
            Profile = profile;
            PlayerName = profile.Name;
            IsOnline = profile.IsOnline;
            AccesKey = accesKey;

            InviteCommand = new RelayCommand(ExecuteInvite);
            this.serviceManager = serviceManager;
            SenderProfile = AccountUtilities.CastAccountProfileToProfileService(serviceManager.ProfileSingleton.Profile);

            LoadProfileImage();
        }

        private void ExecuteInvite(object parameter)
        {
            bool result = serviceManager.ProfileServiceClient.InviteFriendToGame(PlayerName, SenderProfile, AccesKey);

            if(result)
            {
                MessageBox.Show(Utilities.MessageSuccesInviteFriend(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        private void LoadProfileImage()
        {
            try
            {
                ImageSource = ImageManager.LoadProfileImage(Profile, serviceManager);
            }
            catch (InvalidOperationException)
            {
                Utilities.ShowMessageDataBaseUnableToLoad();
            }
        }

    }
}
