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
            });
        }

        public void BroadcastNotifyNewAdmin(int idAdmin)
        {
            Mediator.Notify(Utilities.UPDATE_GAME_ADMIN, idAdmin);
        }

        public void EndGameDisconnectedPlayers()
        {
            MessageBox.Show(Utilities.MessageGameEndNoPlayers(CultureInfo.CurrentCulture.Name), Utilities.TitleGameEnd(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK,MessageBoxImage.Information);
            AccountUtilities.RestartGame();
        }

        public void NotifyGameBoardInitialized(GameBoardStateDto gameBoardState)
        {
            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                List<HexTileDto> hexes = gameBoardState.HexTiles.ToList();
                await Task.Delay(1000);
                Mediator.Notify(Utilities.LOAD_GAME_PLAYER_BOARD, hexes);
            });
 
        }

        public void NotifyPlayerExpulsion(string message, string reason)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = Application.Current.MainWindow;
                mainWindow.IsEnabled = false;

                try
                {
                    if (!string.IsNullOrEmpty(reason))
                    {
                        MessageBox.Show(mainWindow, message + reason, Utilities.MessageGameExpel(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show(mainWindow, Utilities.MessageGameExpelDialog(CultureInfo.CurrentCulture.Name), Utilities.MessageGameExpel(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                    
                finally
                {
                    mainWindow.IsEnabled = true;
                    AccountUtilities.RestartGame();
                }
            });
        }

        public void NotifyPlayerPlacedPiece(GameBoardStateDto gameBoardStateDto, PlayerGameplayDto playerGameplayDto)
        {
            List<HexTileDto> hexes = gameBoardStateDto.HexTiles.ToList();
            Mediator.Notify(Utilities.UPDATE_GAME_PLAYER_BOARD, hexes);
        }

        public void NotifyResourcesDistributed(PlayerResourcesDto receivedResources)
        {
            Mediator.Notify(Utilities.UPDATE_PLAYER_RESOURCES, receivedResources);
        }

        public void NotifyTradeCompletion()
        {
            //TODO Implement this.
        }

        public void NotifyTradeRequest(PlayerResourcesDto needResources, PlayerResourcesDto offerResources)
        {
            List<PlayerResourcesDto> playerResources = new List<PlayerResourcesDto> { needResources, offerResources };
            Mediator.Notify(Utilities.LOAD_GAME_TRADE, playerResources);
        }

        public void SendDiceResult(int diceResult)
        {
                Mediator.Notify(Utilities.SHOW_ROLL_DICE_ANIMATION, diceResult);
        }

        public void SendUpdateConnectedPlayers(OperationResultListOfPlayersInGame listOfPlayers)
        {
            //TODO implement
        }

        public void StartGameForAllPlayers()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mediator.Notify(Utilities.GET_GAME_FOR_SCREEN, null);
            });
        }

        public void UpdateListOfPlayersTurns(PlayerTurnStatusDto[] playersTurnStatus)
        {
            Mediator.Notify(Utilities.LOAD_GAME_PLAYER_LIST, playersTurnStatus);
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mediator.Notify(Utilities.UPDATE_TIME, time);
            });
        }

        public void UpdateTurnStatus(PlayerTurnStatusDto[] playersTurnStatus)
        {
            Mediator.Notify(Utilities.LOAD_GAME_PLAYER_LIST, playersTurnStatus);
        }

        public void UpdateTurnTimeRemaining(int remainingTime)
        {
            Mediator.Notify(Utilities.UPDATE_TIME_GAME, remainingTime);
        }
    }

    
}
