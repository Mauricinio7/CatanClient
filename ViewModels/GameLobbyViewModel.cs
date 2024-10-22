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
        private readonly ServiceManager serviceManager;

        public GameLobbyViewModel(GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;

            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage { Content = game.Name, Name = Utilities.SYSTEM_NAME,IsUserMessage = false }
            };

            var profileDto = serviceManager.ProfileSingleton.Profile;
            profile = new ProfileDto
            {
                Id = profileDto.Id,
                Name = profileDto.Name,
            };

            Mediator.Register("ReceiveMessage", OnReceiveMessage);

            this.serviceManager.ChatServiceClient.JoinChatClient(game, profile);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);
            
        }

        private void OnReceiveMessage(object message)
        {
            var chatMessage = message as ChatMessage;
            if (chatMessage != null)
            {
                Messages.Add(chatMessage);
            }
        }

        private void ExecuteSendMessage()
        {
            serviceManager.ChatServiceClient.SendMessageToServer(game, profile, NewMessage);
        }

 

        

        
    }
    


}


