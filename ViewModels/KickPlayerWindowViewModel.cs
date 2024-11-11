﻿using Autofac;
using CatanClient.Controls;
using CatanClient.GameService;
using CatanClient.ProfileService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.ViewModels
{
    internal class KickPlayerWindowViewModel : ViewModelBase
    {
        public List<GameService.ProfileDto> OnlinePlayers { get; set; } = new List<GameService.ProfileDto>();
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

            result = serviceManager.GameServiceClient.GetPlayerList(AccountUtilities.CastChatGameToGameServiceGame(game));

            if (result.IsSuccess)
            {
                OnlinePlayersList.Clear();
                OnlinePlayers = result.ProfileDtos.ToList();
                


                foreach (var profileDto in OnlinePlayers)
                {
                    OnlinePlayersList.Add(App.Container.Resolve<KickPlayerCardViewModel>(
                        new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto)),
                        new NamedParameter(Utilities.SENDER_PROFILE, AccountUtilities.CastGameProfileToProfileService(AccountUtilities.CastAccountProfileToGameService(profile))),
                        new NamedParameter(Utilities.GAME, AccountUtilities.CastChatGameToGameServiceGame(game))
                        ));
                }

                result.GuestAccountDtos.ToList().ForEach(guest =>
                {
                    OnlinePlayersList.Add(App.Container.Resolve<KickPlayerCardViewModel>(
                        new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(AccountUtilities.CastGuestAccountToGameServiceProfile(guest))),
                        new NamedParameter(Utilities.SENDER_PROFILE, AccountUtilities.CastGameProfileToProfileService(AccountUtilities.CastAccountProfileToGameService(profile))),
                        new NamedParameter(Utilities.GAME, AccountUtilities.CastChatGameToGameServiceGame(game))
                    ));
                });
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }
    }
}
