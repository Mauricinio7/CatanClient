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
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CatanClient.Controls
{
    internal class FriendPlayerCardViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public bool IsOnline { get; set;}
        public ICommand DeleteCommand { get; }

        private ServiceManager serviceManager;
        public ProfileDto SenderProfile { get; set; }
        private BitmapImage imageSource;
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

        public FriendPlayerCardViewModel(ProfileDto profile, ServiceManager serviceManager)
        {
            Profile = profile;
            PlayerName = profile.Name;
            IsOnline = profile.IsOnline;

            DeleteCommand = new RelayCommand(ExecuteDelete);
            this.serviceManager = serviceManager;
            SenderProfile = AccountUtilities.CastAccountProfileToProfileService(serviceManager.ProfileSingleton.Profile);

            LoadProfileImage();
        }

        private void ExecuteDelete(object parameter)
        {
            bool result = serviceManager.ProfileServiceClient.DeleteFriend(PlayerName, SenderProfile);

            if (result)
            {
                MessageBox.Show(Utilities.MessageSuccesRemoveFriend(CultureInfo.CurrentCulture.Name) + PlayerName, Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
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
