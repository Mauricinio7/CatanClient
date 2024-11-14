using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CatanClient.Commands;
using CatanClient.GameService;
using CatanClient.Services;
using CatanClient.UIHelpers;

namespace CatanClient.ViewModels
{
    internal class ExpelPlayerWindowViewModel
    {
        public ObservableCollection<string> ExpulsionReasons { get; set; }
        public string SelectedReason { get; set; }
        public ICommand KickPlayerCommand { get; set; }

        private readonly ServiceManager serviceManager;
        private readonly ProfileService.ProfileDto ExpelPlayer;
        private readonly ProfileService.ProfileDto Profile;
        private readonly GameDto Game;

        public ExpelPlayerWindowViewModel(ProfileService.ProfileDto profile, GameDto game, ServiceManager serviceManager)
        {
            this.ExpelPlayer = profile;
            this.Profile = AccountUtilities.CastAccountProfileToProfileService(serviceManager.ProfileSingleton.Profile);
            this.Game = game;
            this.serviceManager = serviceManager;

            ExpulsionReasons = new ObservableCollection<string>
            {
                "Conducta inapropiada",
                "Inactividad",
                "Lenguaje ofensivo",
                "Trampas detectadas"
            };


            KickPlayerCommand = new AsyncRelayCommand(ExecuteKickPlayerAsync);
        }

        private async Task ExecuteKickPlayerAsync(object parameter)
        {
            GameService.ExpelPlayerDto playerToExpel = new GameService.ExpelPlayerDto
            {
                IdPlayerToExpel = ExpelPlayer.Id.Value,
                Reason = SelectedReason
            };
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
            bool result = await serviceManager.GameServiceClient.ExpelPlayerAsync(playerToExpel, Profile.Id.Value, Game);

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
        }

    }
}