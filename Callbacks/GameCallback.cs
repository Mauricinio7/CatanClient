using CatanClient.GameService;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Callbacks
{
    public class GameCallback : IGameEndPointCallback
    {
            public void BroadcastMessageExpel(PlayerDto playerExpeled)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    ChatMessage systemMessage = new ChatMessage { Content = playerExpeled.profileDto.Name + " ha sido expulsado del juego", Name = Utilities.SYSTEM_NAME, IsUserMessage = false };
                    Mediator.Notify("ReceiveMessage", systemMessage);
                    Mediator.Notify(Utilities.LOAD_PLAYER_LIST, null);
                });
            }
        
        public void NotifyPlayerExpulsion(string message, string reason)
        {
            MessageBox.Show(message + reason, "Ha sido expulsado del juego", MessageBoxButton.OK, MessageBoxImage.Warning);

            AccountUtilities.RestartGame();
        }

        public void StartGameForAll()
        {
            throw new NotImplementedException();
        }
    }

    
}
