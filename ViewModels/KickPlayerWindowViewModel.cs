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
        public List<GameService.ProfileDto> OnlinePlayers { get; set; } = new List<GameService.ProfileDto>();
        public List<GuestAccountDto> OnlinePlayersGuest { get; set; } = new List<GuestAccountDto>();
        public ObservableCollection<KickPlayerCardViewModel> OnlinePlayersList { get; set; } = new ObservableCollection<KickPlayerCardViewModel>();
        private readonly ServiceManager serviceManager;
        private ChatService.GameDto game;
        private AccountService.ProfileDto profile;

        public KickPlayerWindowViewModel(ChatService.GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;

            profile = serviceManager.ProfileSingleton.Profile;
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

                    if (OnlinePlayers.Count > 0 && OnlinePlayers != null)
                    {
                        foreach (var profileDto in OnlinePlayers)
                        {
                            OnlinePlayersList.Add(App.Container.Resolve<KickPlayerCardViewModel>(
                            new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto)),
                            new NamedParameter(Utilities.GAME, AccountUtilities.CastChatGameToGameServiceGame(game))
                        ));
                        }
                    }

                    if (OnlinePlayersGuest.Count > 0 && OnlinePlayersGuest != null)
                    {
                        foreach (var guestProfileDto in OnlinePlayersGuest)
                        {
                            GameService.ProfileDto profileDto = AccountUtilities.CastGuestAccountToGameServiceProfile(guestProfileDto);
                            OnlinePlayersList.Add(App.Container.Resolve<KickPlayerCardViewModel>(
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
