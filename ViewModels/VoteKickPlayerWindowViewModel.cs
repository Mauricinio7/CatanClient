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

                    if (OnlinePlayers.Count > 0)
                    {
                        foreach (var profileDto in OnlinePlayers)
                        {
                            OnlinePlayersList.Add(App.Container.Resolve<VoteKickPlayerCardViewModel>(
                            new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto)),
                            new NamedParameter(Utilities.GAME, AccountUtilities.CastChatGameToGameServiceGame(game))
                        ));
                        }
                    }

                    if (OnlinePlayersGuest.Count > 0)
                    {
                        foreach (var guestProfileDto in OnlinePlayersGuest)
                        {
                            GameService.ProfileDto profileDto = AccountUtilities.CastGuestAccountToGameServiceProfile(guestProfileDto);
                            OnlinePlayersList.Add(App.Container.Resolve<VoteKickPlayerCardViewModel>(
                             new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto)),
                             new NamedParameter(Utilities.GAME, AccountUtilities.CastChatGameToGameServiceGame(game))
                         ));
                        }
                    }
                }
                else
                {
                    Utilities.ShowMessgeServerLost();
                }
            });  
        }
    }
}
