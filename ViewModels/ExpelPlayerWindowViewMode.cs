using System.Collections.ObjectModel;
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

        public ExpelPlayerWindowViewModel(ProfileService.ProfileDto expelPlayer, ProfileService.ProfileDto senderProfile, GameDto game, ServiceManager serviceManager)
        {
            this.ExpelPlayer = expelPlayer;
            this.Profile = senderProfile;
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
                MessageBox.Show("Jugador expulsado");
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

    }
}