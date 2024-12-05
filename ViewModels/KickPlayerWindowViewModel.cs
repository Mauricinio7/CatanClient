using Autofac;
using CatanClient.AccountService;
using CatanClient.Controls;
using CatanClient.GameService;
using CatanClient.ProfileService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.ViewModels
{
    internal class KickPlayerWindowViewModel : ViewModelBase
    {
        public ObservableCollection<KickPlayerCardViewModel> OnlinePlayersList { get; set; } = new ObservableCollection<KickPlayerCardViewModel>();
        private readonly ServiceManager serviceManager;
        private readonly ChatService.GameDto game;

        public KickPlayerWindowViewModel(ChatService.GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;
            LoadPlayerList();
        }

        internal void LoadPlayerList()
        {
            List<GameService.ProfileDto> OnlinePlayers = new List<GameService.ProfileDto>();
            List<GuestAccountDto> OnlinePlayersGuest = new List<GuestAccountDto>();
            OperationResultListOfPlayersInGame result;

            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                result = await serviceManager.GameServiceClient.GetPlayerListAsync(AccountUtilities.CastChatGameToGameServiceGame(game));

                if (result.IsSuccess)
                {
                    OnlinePlayersList.Clear();
                    OnlinePlayers = result.ProfileDtos.ToList();
                    OnlinePlayersGuest = result.GuestAccountDtos.ToList();

                    AddPlayersToOnlineList(OnlinePlayers, OnlinePlayersGuest);
                }
                else
                {
                    Utilities.ShowMessgeServerLost();
                }
            });
        }

        private void AddPlayersToOnlineList(IEnumerable<GameService.ProfileDto> registeredPlayers, IEnumerable<GuestAccountDto> guestPlayers)
        {
            AddRegisteredPlayers(registeredPlayers);
            AddGuestPlayers(guestPlayers);
        }

        private void AddRegisteredPlayers(IEnumerable<GameService.ProfileDto> registeredPlayers)
        {
            foreach (var profileDto in registeredPlayers)
            {
                OnlinePlayersList.Add(App.Container.Resolve<KickPlayerCardViewModel>(
                    new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto)),
                    new NamedParameter(Utilities.GAME, AccountUtilities.CastChatGameToGameServiceGame(game))));
            }
        }

        private void AddGuestPlayers(IEnumerable<GuestAccountDto> guestPlayers)
        {
            foreach (var guestProfileDto in guestPlayers)
            {
                var profileDto = AccountUtilities.CastGuestAccountToGameServiceProfile(guestProfileDto);
                OnlinePlayersList.Add(App.Container.Resolve<KickPlayerCardViewModel>(
                    new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto)),
                    new NamedParameter(Utilities.GAME, AccountUtilities.CastChatGameToGameServiceGame(game))));
            }
        }
    }
}
