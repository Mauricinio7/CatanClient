using CatanClient.Callbacks;
using CatanClient.ChatService;
using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.Singleton;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private ProfileDto profile;


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
        private readonly ServiceManager serviceManager;

        public GameLobbyViewModel(GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;

            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage { Content = game.Name, Name = Utilities.SYSTEM_NAME,IsUserMessage = false }
            };

            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profile = new ProfileDto
            {
                Id = profileDto.Id,
                Name = profileDto.Name,
            };

            Mediator.Register("ReceiveMessage", OnReceiveMessage);

            this.serviceManager.ChatServiceClient.JoinChatClient(game, profile);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);
            LeftRoomCommand = new RelayCommand(ExecuteLeftRoom);

        }

        internal void OnReceiveMessage(object message)
        {
            ChatMessage chatMessage = message as ChatMessage;
            if (chatMessage != null)
            {
                Messages.Add(chatMessage);
            }
        }

        internal void ExecuteSendMessage()
        {
            serviceManager.ChatServiceClient.SendMessageToServer(game, profile, NewMessage);
        }

        internal void ExecuteLeftRoom()
        {
            serviceManager.ChatServiceClient.LeftChatClient(game, profile);

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
                PicturePath = profile.PicturePath,
                PreferredLanguage = profile.PreferredLanguage
            };


            serviceManager.GameServiceClient.LeftRoomClient(gameRoom, profileRoom);

            Mediator.Notify(Utilities.SHOWMAINMENU, null);
        }
        





    }
    


}


