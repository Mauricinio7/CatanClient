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
