using CatanClient.ProfileService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.ViewModels;
using System;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace CatanClient.Controls
{
    internal class PlayerInGameCardViewModel : ViewModelBase
    {

        public string PlayerName { get; set; }
        public string Points { get; set; }
        public bool Turn { get; set; }

        private readonly ServiceManager serviceManager;

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

        public PlayerInGameCardViewModel(ProfileDto profile, int points, bool turn, ServiceManager serviceManager)
        {
            Profile = profile;
            PlayerName = profile.Name;
            Turn = turn;
            this.serviceManager = serviceManager;

            Points = Utilities.LabelPoints(CultureInfo.CurrentCulture.Name) + ": " + points.ToString(); 

            LoadProfileImage();

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
