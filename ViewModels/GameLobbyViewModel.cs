using CatanClient.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class GameLobbyViewModel : ViewModelBase
    {
        private string _newMessage;
        private Timer _systemMessageTimer;


        public ObservableCollection<ChatMessage> Messages { get; set; }


        public string NewMessage
        {
            get { return _newMessage; }
            set
            {
                _newMessage = value;
                OnPropertyChanged(nameof(NewMessage));
            }
        }

        public ICommand SendMessageCommand { get; }

        public GameLobbyViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage { Content = "Bienvenido al chat", IsUserMessage = false }
            };

            SendMessageCommand = new RelayCommand(o => SendMessage());
            _systemMessageTimer = new Timer(10000);  
            _systemMessageTimer.Elapsed += SendSystemMessage;
            _systemMessageTimer.Start();
        }

        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(NewMessage))
            {
                Messages.Add(new ChatMessage { Content = NewMessage, IsUserMessage = true });
                NewMessage = string.Empty;
            }
        }

        private void ReciveMessageCallback(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Messages.Add(new ChatMessage { Content = message, IsUserMessage = false });
                NewMessage = string.Empty;
            }
        }

        private void SendSystemMessage(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Messages.Add(new ChatMessage { Content = "Mensaje automático del sistema", IsUserMessage = false });
            });
        }
    }
    public class ChatMessage
    {
        public string Content { get; set; }
        public bool IsUserMessage { get; set; }  
    }
}


