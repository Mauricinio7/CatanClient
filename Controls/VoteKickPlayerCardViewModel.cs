using CatanClient.Commands;
using CatanClient.ProfileService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.ViewModels;
using CatanClient.Views;
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
    internal class VoteKickPlayerCardViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public ICommand VoteKickCommand { get; }

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

        public VoteKickPlayerCardViewModel(ProfileDto profile, GameService.GameDto game, ServiceManager serviceManager)
        {
            Profile = profile;
            PlayerName = profile.Name;
            Game = game;

            VoteKickCommand = new RelayCommand(_ => ExecuteVoteKick(null), _ => CanKickPlayer(null));

            this.serviceManager = serviceManager;
            SenderProfile = AccountUtilities.CastAccountProfileToProfileService(serviceManager.ProfileSingleton.Profile);

            if (Profile.IsRegistered)
            {
                LoadProfileImage();
            }
            else
            {
                LoadGuestImage();
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
            string searchPattern = Utilities.ProfilePhotoPathDeleteVersion(playerId);
            foreach (var file in Directory.GetFiles(appDirectory, searchPattern))
            {
                File.Delete(file);
            }
        }

        private bool CanKickPlayer(object parameter)
        {
            return Profile.Id != SenderProfile.Id;
        }

        private void ExecuteVoteKick(object parameter)
        {
            GameService.ExpelPlayerDto playerToExpel = new GameService.ExpelPlayerDto
            {
                IdPlayerToExpel = Profile.Id.Value,
            };
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                bool result = await serviceManager.GameServiceClient.VoteExpelPlayerAsync(playerToExpel, SenderProfile.Id.Value, Game);
                if (result)
                {
                    MessageBox.Show(Utilities.MessageSuccesKickPlayer(CultureInfo.CurrentCulture.Name), Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
                    Mediator.Notify(Utilities.CLOSE_EXPEL_PLAYER, null);
                    Mediator.Notify(Utilities.CLOSE_KICK_PLAYER, null);
                }
                else
                {
                    Utilities.ShowMessgeServerLost();
                }
            });
        }
    }
}
