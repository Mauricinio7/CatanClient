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
            string appDirectory = Path.Combine(Environment.CurrentDirectory, Utilities.PROFILE_IMAGE_DIRECTORY);

            string fileName = Utilities.ProfilePhotoPathWithVersion(Profile.Id.Value, Profile.PictureVersion);
            string imagePath = Path.Combine(appDirectory, fileName);

            if (File.Exists(imagePath))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap.EndInit();

                ImageSource = bitmap;
            }
            else
            {
                ProfileService.OperationResultPictureDto result;
                result = serviceManager.ProfileServiceClient.GetFriendImage(Profile);

                if (result.IsSuccess)
                {
                    byte[] imageBytes = result.Picture;

                    DeleteOldVersions(Profile.Id.Value);
                    SaveImageBytesLocally(imageBytes);

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap.EndInit();

                    ImageSource = bitmap;
                }
                else
                {
                    Utilities.ShowMessageDataBaseUnableToLoad();
                }
            }
        }

        private void SaveImageBytesLocally(byte[] imageBytes)
        {
            string appDirectory = Path.Combine(Environment.CurrentDirectory, Utilities.PROFILE_IMAGE_DIRECTORY);

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            string fileName = Utilities.ProfilePhotoPathWithVersion(Profile.Id.Value, Profile.PictureVersion);
            string destinationPath = Path.Combine(appDirectory, fileName);

            File.WriteAllBytes(destinationPath, imageBytes);
        }

        private void DeleteOldVersions(int playerId)
        {
            string appDirectory = Path.Combine(Environment.CurrentDirectory, Utilities.PROFILE_IMAGE_DIRECTORY);
            string searchPattern =Utilities.ProfilePhotoPathDeleteVersion(playerId);
            foreach (var file in Directory.GetFiles(appDirectory, searchPattern))
            {
                File.Delete(file);
            }
        }

    }
}
