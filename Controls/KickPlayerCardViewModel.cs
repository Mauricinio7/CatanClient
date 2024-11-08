using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CatanClient.UIHelpers;
using System.IO;
using System.Windows.Media.Imaging;
using CatanClient.ProfileService;
using CatanClient.Views;

namespace CatanClient.Controls
{
    internal class KickPlayerCardViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public ICommand KickCommand { get; }

        private ServiceManager serviceManager;
        public ProfileDto SenderProfile { get; set; }
        private BitmapImage imageSource;
        private ProfileDto Profile { get; set; }
        private GameService.GameDto Game { get; set; }

        public BitmapImage ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public KickPlayerCardViewModel(ProfileDto profile, ProfileDto senderProfile, GameService.GameDto game,ServiceManager serviceManager)
        {
            Profile = profile;
            SenderProfile = senderProfile;
            PlayerName = profile.Name;
            Game = game;

            KickCommand = new RelayCommand(ExecuteKick);
            this.serviceManager = serviceManager;
            SenderProfile = AccountUtilities.CastAccountProfileToProfileService(serviceManager.ProfileSingleton.Profile);

            LoadProfileImage();
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
            string searchPattern = $"ProfilePhoto{playerId}_V*.jpg";
            foreach (var file in Directory.GetFiles(appDirectory, searchPattern))
            {
                File.Delete(file);
            }
        }

        private void ExecuteKick(object parameter)
        {
            var expelWindow = new ExpelPlayerWindow(Profile, SenderProfile, Game);

            expelWindow.ShowDialog();
        }
    }
}
