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
