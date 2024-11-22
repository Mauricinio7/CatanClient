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
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CatanClient.Controls
{
    internal class FriendRequestPlayerCardViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public bool IsOnline { get; set; }
        public ICommand AcceptCommand { get; }
        public ICommand RejectCommand { get; }

        private readonly ServiceManager serviceManager;
        public ProfileDto ReciverProfile { get; set; }
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

        public FriendRequestPlayerCardViewModel(ProfileDto profile, ServiceManager serviceManager)
        {
            Profile = profile;
            PlayerName = profile.Name;
            IsOnline = profile.IsOnline;

            this.serviceManager = serviceManager;
            ReciverProfile = AccountUtilities.CastAccountProfileToProfileService(serviceManager.ProfileSingleton.Profile);

            AcceptCommand = new RelayCommand(ExecuteAccept);
            RejectCommand = new RelayCommand(ExecuteReject);

            LoadProfileImage();
        }

        private void ExecuteAccept(object parameter)
        {
            bool result = serviceManager.ProfileServiceClient.AcceptFriendRequest(PlayerName, ReciverProfile);

            if (result)
            {
                MessageBox.Show(Utilities.MessageSuccesAddFriend(CultureInfo.CurrentCulture.Name) + PlayerName, Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        private void ExecuteReject(object parameter)
        {
            bool result = serviceManager.ProfileServiceClient.RejectFriendRequest(PlayerName, ReciverProfile);

            if (result)
            {
                MessageBox.Show(Utilities.MessageSuccesRejectFriend(CultureInfo.CurrentCulture.Name) + PlayerName, Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
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
