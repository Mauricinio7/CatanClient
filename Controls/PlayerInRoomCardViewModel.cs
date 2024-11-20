using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;
using CatanClient.ProfileService;
using System.Globalization;

namespace CatanClient.Controls
{
    internal class PlayerInRoomCardViewModel : ViewModelBase
    {

        public string PlayerName { get; set; }
        public string Ready { get; set; }

        private ServiceManager serviceManager;

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

        public PlayerInRoomCardViewModel(ProfileDto profile, ServiceManager serviceManager)
        {
            Profile = profile;
            PlayerName = profile.Name;
            SetReady();
            this.serviceManager = serviceManager;


                LoadProfileImage();
            
        }

        internal void SetReady()
        {
            if (Profile.isReadyToPlay) 
            {
                Ready = Utilities.GlobalReady(CultureInfo.CurrentCulture.Name);
            }
            else
            {
                Ready = Utilities.GlobalNoReady(CultureInfo.CurrentCulture.Name);
            }
        }


        private void LoadProfileImage()
        {
            if (!Profile.IsRegistered)
            {
                LoadGuestImage();
            }
            else
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
        }

        private void LoadGuestImage()
        {
            string filePath = Utilities.GetDefaultPhotoPath();

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.EndInit();

            ImageSource = bitmap;
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
            string searchPattern = Utilities.ProfilePhotoPathDeleteVersion(playerId);
            foreach (var file in Directory.GetFiles(appDirectory, searchPattern))
            {
                File.Delete(file);
            }
        }
    }
}
