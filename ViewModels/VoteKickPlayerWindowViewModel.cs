using Autofac;
using CatanClient.Controls;
using CatanClient.GameService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.ViewModels
{
    internal class VoteKickPlayerWindowViewModel : ViewModelBase
    {
        public List<GameService.ProfileDto> OnlinePlayers { get; set; } = new List<GameService.ProfileDto>();
        public List<GuestAccountDto> OnlinePlayersGuest { get; set; } = new List<GuestAccountDto>();
        public ObservableCollection<VoteKickPlayerCardViewModel> OnlinePlayersList { get; set; } = new ObservableCollection<VoteKickPlayerCardViewModel>();
        private readonly ServiceManager serviceManager;
        private readonly ChatService.GameDto game;

        public VoteKickPlayerWindowViewModel(ChatService.GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;

            LoadPlayerList();
        }

        internal void LoadPlayerList()
        {
            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                var result = await serviceManager.GameServiceClient.GetPlayerListAsync(AccountUtilities.CastChatGameToGameServiceGame(game));

                if (result.IsSuccess)
                {
                    OnlinePlayersList.Clear();
                    OnlinePlayers = result.ProfileDtos.ToList();
                    OnlinePlayersGuest = result.GuestAccountDtos.ToList();

                    AddPlayersToList(OnlinePlayers, OnlinePlayersGuest);
                }
                else
                {
                    Utilities.ShowMessgeServerLost();
                }
            });
        }

        private void AddPlayersToList(IEnumerable<ProfileDto> registeredPlayers, IEnumerable<GuestAccountDto> guestPlayers)
        {
            AddRegisteredPlayers(registeredPlayers);
            AddGuestPlayers(guestPlayers);
        }

        private void AddRegisteredPlayers(IEnumerable<ProfileDto> registeredPlayers)
        {
            foreach (var profileDto in registeredPlayers)
            {
                OnlinePlayersList.Add(App.Container.Resolve<VoteKickPlayerCardViewModel>(
                    new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto)),
                    new NamedParameter(Utilities.GAME, AccountUtilities.CastChatGameToGameServiceGame(game))));
            }
        }

        private void AddGuestPlayers(IEnumerable<GuestAccountDto> guestPlayers)
        {
            foreach (var guestProfileDto in guestPlayers)
            {
                var profileDto = AccountUtilities.CastGuestAccountToGameServiceProfile(guestProfileDto);
                OnlinePlayersList.Add(App.Container.Resolve<VoteKickPlayerCardViewModel>(
                    new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto)),
                    new NamedParameter(Utilities.GAME, AccountUtilities.CastChatGameToGameServiceGame(game))));
            }
        }
    }
}
