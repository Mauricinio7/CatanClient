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
            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                string playerName = playerExpeled.profileDto?.Name ?? playerExpeled.guestAccountDto?.Name;

                if (!string.IsNullOrEmpty(playerName))
                {
                    string messageContent = playerName + " " + Utilities.MessageGameExpel(CultureInfo.CurrentCulture.Name);
                    ChatMessage systemMessage = new ChatMessage
                    {
                        Content = messageContent,
                        Name = Utilities.SYSTEM_NAME,
                        IsUserMessage = false
                    };

                    Mediator.Notify(Utilities.RECIVE_MESSAGE, systemMessage);
                    await Task.Delay(3000);
                    Mediator.Notify(Utilities.LOAD_PLAYER_LIST, null);
                }
                else
                {
                    Console.WriteLine("No se pudo obtener el nombre del jugador expulsado.");
                }
            });
        }

        public void BroadcastNotifyNewAdmin(int idAdmin)
        {
            throw new NotImplementedException();
        }

        public void NotifyPlayerExpulsion(string message, string reason)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = Application.Current.MainWindow;
                mainWindow.IsEnabled = false;

                try
                {
                    MessageBox.Show(mainWindow, message + reason, Utilities.MessageGameExpel(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    mainWindow.IsEnabled = true;
                    AccountUtilities.RestartGame();
                }
            });
        }

        public void StartGameForAllPlayers()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mediator.Notify(Utilities.SHOW_GAME_SCREEN, null);
            });

            
        }

        public void UpdateTimeToStartGame(int time)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mediator.Notify(Utilities.UPDATE_TIME, time);
            });
        }

        public void UpdateTimeWhenJoinGame(int time)
        {
            throw new NotImplementedException();
        }

        public void UpdateTurnStatus(PlayerTurnStatusDto[] playersTurnStatus)
        {
            throw new NotImplementedException();
        }
    }

    
}
