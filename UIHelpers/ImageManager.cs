using CatanClient.ProfileService;
using CatanClient.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CatanClient.UIHelpers
{
    public static class ImageManager
    {
        internal static BitmapImage LoadProfileImage(ProfileDto profile, ServiceManager serviceManager)
        {
            BitmapImage image = null; 

            if (!profile.IsRegistered)
            {
                image = LoadGuestImage();
            }
            else
            {
                string appDirectory = Path.Combine(Environment.CurrentDirectory, Utilities.PROFILE_IMAGE_DIRECTORY);

                BitmapImage localImage = LoadImageLocally(profile.Id.Value, profile.PictureVersion, appDirectory);
                if (localImage != null)
                {
                    image = localImage;
                }
                else
                {
                    ProfileService.OperationResultPictureDto result = serviceManager.ProfileServiceClient.GetFriendImage(profile);
                    if (result.IsSuccess)
                    {
                        byte[] imageBytes = result.Picture;

                        DeleteOldVersions(profile.Id.Value, appDirectory);
                        SaveImageBytesLocally(profile.Id.Value, profile.PictureVersion, imageBytes, appDirectory);

                        image = LoadImageLocally(profile.Id.Value, profile.PictureVersion, appDirectory);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            return image; 
        }

        private static BitmapImage LoadImageLocally(int profileId, int pictureVersion, string appDirectory)
        {
            string fileName = Utilities.ProfilePhotoPathWithVersion(profileId, pictureVersion);
            string imagePath = Path.Combine(appDirectory, fileName);
            BitmapImage bitmap = null;

            if (File.Exists(imagePath))
            {
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap.EndInit();
            }

            return bitmap;
        }

        private static void SaveImageBytesLocally(int profileId, int pictureVersion, byte[] imageBytes, string appDirectory)
        {
            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            string fileName = Utilities.ProfilePhotoPathWithVersion(profileId, pictureVersion);
            string destinationPath = Path.Combine(appDirectory, fileName);

            File.WriteAllBytes(destinationPath, imageBytes);
        }

        private static void DeleteOldVersions(int profileId, string appDirectory)
        {
            string searchPattern = Utilities.ProfilePhotoPathDeleteVersion(profileId);
            foreach (var file in Directory.GetFiles(appDirectory, searchPattern))
            {
                File.Delete(file);
            }
        }

        private static BitmapImage LoadGuestImage()
        {
            string filePath = Utilities.GetDefaultPhotoPath();

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.EndInit();

            return bitmap;
        }
    }
}
