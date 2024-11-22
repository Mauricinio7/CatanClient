﻿using CatanClient.Commands;
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

        public KickPlayerCardViewModel(ProfileDto profile, GameService.GameDto game,ServiceManager serviceManager)
        {
            Profile = profile;
            PlayerName = profile.Name;
            Game = game;

            KickCommand = new RelayCommand(_ => ExecuteKick(null), _ => CanKickPlayer(null));

            this.serviceManager = serviceManager;
            SenderProfile = AccountUtilities.CastAccountProfileToProfileService(serviceManager.ProfileSingleton.Profile);
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

        private bool CanKickPlayer(object parameter)
        {
            return Profile.Id != SenderProfile.Id;
        }

        private void ExecuteKick(object parameter)
        {
            var expelWindow = new ExpelPlayerWindow(Profile, Game);

            expelWindow.ShowDialog();
        }
    }
}
