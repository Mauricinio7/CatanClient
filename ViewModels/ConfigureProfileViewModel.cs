using CatanClient.AccountService;
using CatanClient.Commands;
using CatanClient.Properties;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.Views;
using Microsoft.Win32;
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

namespace CatanClient.ViewModels
{
    internal class ConfigureProfileViewModel : ViewModelBase
    {
        public ICommand ModifyProfileCommand { get; }
        public ICommand ModifyPasswordCommand { get; }
        public ICommand SelectImageCommand { get; }


        private string username;
        private AccountDto account;
        private string email;
        private string phone;
        private ProfileDto profile;
        private BitmapImage imageSource;
        private readonly ServiceManager serviceManager;



        public BitmapImage ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }


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
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));  
            }
        }

        public string Email
        
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));  
            }
        }


        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));  
            }
        }

        public ProfileDto Profile { get => profile; set => profile = value; }

       
        public ConfigureProfileViewModel(AccountDto account, ServiceManager serviceManager)
        {
            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profileDto.PreferredLanguage = CultureInfo.CurrentCulture.Name;
            Profile = profileDto;
            ModifyProfileCommand = new RelayCommand(OnModifyProfile);
            ModifyPasswordCommand = new RelayCommand(OnModifyPassword);
            SelectImageCommand = new RelayCommand(OpenFileDialog);
            this.serviceManager = serviceManager;
            Account = account;
            Username = Profile.Name;
            Email = account.Email;
            Phone = account.PhoneNumber;
            LoadProfileImage();
        }

        private void OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = Utilities.IMAGE_FILTER,
                Title = Utilities.TitleImageSelector(CultureInfo.CurrentCulture.Name)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;


                FileInfo fileInfo = new FileInfo(selectedFilePath);
                if (fileInfo.Length > 6 * 1024 * 1024) 
                {
                    MessageBox.Show(Utilities.MessageTooBigFile(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFilePath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                SaveImageInServer(selectedFilePath);
                LoadProfileImage();
            }
        }

        private void SaveImageInServer(string filePath)
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);

            ProfileService.OperationResultProfileDto result;

            ProfileService.ProfileDto profileDto = AccountUtilities.CastAccountProfileToProfileService(Profile);
            
            result = serviceManager.ProfileServiceClient.UploadImage(profileDto, imageBytes);

            if (result.IsSuccess)
            { 
                SaveImageLocally(filePath);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }

        }

        private void SaveImageLocally(string filePath)
        {
            string appDirectory = Path.Combine(Environment.CurrentDirectory, Utilities.PROFILE_IMAGE_DIRECTORY);

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            string fileName = Utilities.ProfilePhotoPath(profile.Id.Value);
            string destinationPath = Path.Combine(appDirectory, fileName);

            File.Copy(filePath, destinationPath, overwrite: true);
        }

        private void LoadProfileImage()
        {
            string appDirectory = Path.Combine(Environment.CurrentDirectory, Utilities.PROFILE_IMAGE_DIRECTORY);

            string fileName = Utilities.ProfilePhotoPath(profile.Id.Value);
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
                ProfileService.ProfileDto profileDto = AccountUtilities.CastAccountProfileToProfileService(Profile);
                ProfileService.OperationResultPictureDto result;
                result = serviceManager.ProfileServiceClient.GetImage(profileDto);

                if (result.IsSuccess)
                {
                    byte[] imageBytes = result.Picture;

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

            string fileName = Utilities.ProfilePhotoPath(profile.Id.Value);
            string destinationPath = Path.Combine(appDirectory, fileName);

            File.WriteAllBytes(destinationPath, imageBytes);
        }




        private void OnModifyProfile(object parameter)
        {
            if (parameter is string field)
            {
                var editWindow = new EditProfileWindow(field);

                editWindow.ShowDialog();
            }
        }

        private void OnModifyPassword(object parameter)
        {
                var editWindow = new EditPasswordWindow();

                editWindow.ShowDialog();
        }
    }
}
