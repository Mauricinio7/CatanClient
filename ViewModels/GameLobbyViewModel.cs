﻿using Autofac;
using CatanClient.Callbacks;
using CatanClient.ChatService;
using CatanClient.Commands;
using CatanClient.Controls;
using CatanClient.GameService;
using CatanClient.Services;
using CatanClient.Singleton;
using CatanClient.UIHelpers;
using CatanClient.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class GameLobbyViewModel : ViewModelBase
    {
        private string newMessage;
        private ChatService.GameDto game;
        private AccountService.ProfileDto profile;
        public List<ProfileDto> OnlinePlayers { get; set; } = new List<ProfileDto>();
        public ObservableCollection<PlayerInRoomCardViewModel> OnlinePlayersList { get; set; } = new ObservableCollection<PlayerInRoomCardViewModel>();


        public ObservableCollection<ChatMessage> Messages { get; set; }


        public string NewMessage
        {
            get { return newMessage; }
            set
            {
                newMessage = value;
                OnPropertyChanged(nameof(NewMessage));
            }
        }

        public ICommand SendMessageCommand { get; }
        public ICommand LeftRoomCommand { get; }
        public ICommand KickPlayerCommand { get; }
        public ICommand ShowInviteFriendCommand { get; }
        private readonly ServiceManager serviceManager;

        public GameLobbyViewModel(ChatService.GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;

            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage { Content = game.Name + " ID: " + game.AccessKey, Name = Utilities.SYSTEM_NAME,IsUserMessage = false }
            };

            profile =  serviceManager.ProfileSingleton.Profile;

            Mediator.Register(Utilities.RECIVEMESSAGE, OnReceiveMessage);
            Mediator.Register(Utilities.LOAD_PLAYER_LIST, LoadPlayerList);

            this.serviceManager.ChatServiceClient.JoinChatClient(game, profile.Name);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);
            LeftRoomCommand = new RelayCommand(ExecuteLeftRoom);
            KickPlayerCommand = new RelayCommand(ExecuteShowKickPlayer);
            ShowInviteFriendCommand= new RelayCommand(ExecuteShowInviteFriend);
        }

        internal void LoadPlayerList(object parameter)
        {
            OperationResultListOfPlayersInGame result;

            result = serviceManager.GameServiceClient.GetPlayerList(AccountUtilities.CastChatGameToGameServiceGame(game));

            if (result.IsSuccess)
            {
                OnlinePlayers = result.ProfileDtos.ToList();


                foreach (var profileDto in OnlinePlayers)
                {
                    OnlinePlayersList.Add(App.Container.Resolve<PlayerInRoomCardViewModel>(
                        new NamedParameter("profile", AccountUtilities.CastGameProfileToProfileService(profileDto))));
                }
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        internal void OnReceiveMessage(object message)
        {
            ChatMessage chatMessage = message as ChatMessage;
            if (chatMessage != null)
            {
                Messages.Add(chatMessage);
            }
        }

        internal void ExecuteShowKickPlayer()
        {
            var kickPlayerWindow = new KickPlayerWindow();

            kickPlayerWindow.ShowDialog();
        }

        internal void ExecuteShowInviteFriend()
        {
            Mediator.Notify(Utilities.SHOW_INVITE_FRIENDS, game.AccessKey);
        }


        internal void ExecuteSendMessage()
        {
            serviceManager.ChatServiceClient.SendMessageToServer(game, profile.Name, NewMessage);
        }

        internal void ExecuteLeftRoom()
        {
            serviceManager.ChatServiceClient.LeftChatClient(game, profile.Name);

            GameService.GameDto gameRoom = new GameService.GameDto
            {
                Name = game.Name,
                Id = game.Id,
                AccessKey = game.AccessKey,
                MaxNumberPlayers = game.MaxNumberPlayers
            };

            GameService.ProfileDto profileRoom = new GameService.ProfileDto
            {
                Name = profile.Name,
                Id = profile.Id,
                CurrentSessionID = profile.CurrentSessionID,
                PreferredLanguage = CultureInfo.CurrentCulture.Name
            };


            serviceManager.GameServiceClient.LeftRoomClient(gameRoom, profileRoom);
            LoadPlayerList(null);

            AccountUtilities.RestartGame();
        }

    }
    


}


