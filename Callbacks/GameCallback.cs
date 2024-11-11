using CatanClient.GameService;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                    ChatMessage systemMessage = new ChatMessage { Content = playerExpeled.profileDto.Name + " " + Utilities.MessageGameExpel(CultureInfo.CurrentCulture.Name), Name = Utilities.SYSTEM_NAME, IsUserMessage = false };
                    Mediator.Notify(Utilities.RECIVEMESSAGE, systemMessage);
                    Mediator.Notify(Utilities.LOAD_PLAYER_LIST, null);
                });
            }
        
        public void NotifyPlayerExpulsion(string message, string reason)
        {
            MessageBox.Show(message + reason, Utilities.MessageGameExpel(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);

            AccountUtilities.RestartGame();
        }

        public void StartGameForAll()
        {
            throw new NotImplementedException();
        }
    }

    
}
