using CatanClient.Callbacks;
using CatanClient.ChatService;
using CatanClient.Commands;
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
        private GameDto game;
        private AccountService.ProfileDto profile;


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
        private readonly ServiceManager serviceManager;

        public GameLobbyViewModel(GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;

            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage { Content = game.Name + " ID: " + game.AccessKey, Name = Utilities.SYSTEM_NAME,IsUserMessage = false }
            };

            profile =  serviceManager.ProfileSingleton.Profile;

            Mediator.Register(Utilities.RECIVEMESSAGE, OnReceiveMessage);

            this.serviceManager.ChatServiceClient.JoinChatClient(game, profile.Name);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);
            LeftRoomCommand = new RelayCommand(ExecuteLeftRoom);
            KickPlayerCommand = new RelayCommand(ExecuteShowKickPlayer);

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

            AccountUtilities.RestartGame();
        }

    }
    


}


