using Autofac;
using CatanClient.Callbacks;
using CatanClient.GameService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Globalization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Services
{
    public class GameServiceClient : IGameServiceClient
    {
        private DuplexChannelFactory<IGameEndPoint> channelFactory;
        private IGameEndPoint client;
        private InstanceContext callbackInstance;

        public GameServiceClient()
        {
            callbackInstance = new InstanceContext(new GameCallback());
        }

        private void OpenConnection()
        {
            if (client != null) return;

            var binding = new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None },
                MaxBufferSize = 485760,
                MaxReceivedMessageSize = 485760,
                OpenTimeout = TimeSpan.FromMinutes(1),
                CloseTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(2),
                ReceiveTimeout = TimeSpan.FromMinutes(10)
            };

            var endpoint = new EndpointAddress(Utilities.IP_GAME_SERVICE);
            channelFactory = new DuplexChannelFactory<IGameEndPoint>(callbackInstance, binding, endpoint);
            client = channelFactory.CreateChannel();
        }

        private void CloseConnection()
        {
            if (client != null)
            {
                try
                {
                    ((ICommunicationObject)client).Close();
                }
                catch (Exception ex)
                {
                    Log.Error($"Error al cerrar la conexión: {ex.Message}");
                    ((ICommunicationObject)client).Abort();
                }
                finally
                {
                    client = null;
                    channelFactory = null;
                }
            }
        }

        public async Task<OperationResultGameDto> CreateRoomClientAsync(GameDto game, ProfileDto profile)
        {
            OpenConnection();
            OperationResultGameDto result;
            try
            {
                result = await client.CreateGameAsync(game, profile);
            }
            catch (Exception ex)
            {
                result = new OperationResultGameDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };
                Log.Error(ex.Message);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<bool> StartGameAsync(PlayerGameplayDto playerGameplayDto, GameDto gameClientDto)
        {
            OpenConnection();
            bool result = false;
            try
            {
                result = await client.StartGameAsync(playerGameplayDto, gameClientDto);
            }
            catch (Exception ex)
            {

                Log.Error(ex.Message);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultGameDto> JoinRoomClientAsync(string code, ProfileDto profile)
        {
            OpenConnection();
            OperationResultGameDto result;
            try
            {
                result = await client.JoinGameAsync(code, profile);
            }
            catch (Exception ex)
            {
                result = new OperationResultGameDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };
                Log.Error(ex.Message);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultGameDto> JoinRoomAsGuestClientAsync(string code, GuestAccountDto profile)
        {
            OpenConnection();
            OperationResultGameDto result;
            try
            {
                result = await client.JoinGameAsaGuestAsync(code, profile);
            }
            catch (Exception ex)
            {
                result = new OperationResultGameDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };
                Log.Error(ex.Message);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<bool> LeftRoomClientAsync(GameDto game, ProfileDto profile)
        {
            OpenConnection();
            bool result = false;
            try
            {
                result = (await client.QuitGameAsync(game, profile)).IsSuccess;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<bool> LeftRoomGuestClientAsync(GameDto game, GuestAccountDto guest)
        {
            OpenConnection();
            bool result = false;
            try
            {
                result = (await client.QuitGameAsaGuestAccountAsync(game, guest)).IsSuccess;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultListOfPlayersInGame> GetPlayerListAsync(GameDto game)
        {
            OpenConnection();
            OperationResultListOfPlayersInGame result;
            try
            {
                
                result = await client.GetAllPlayersInGameAsync(game, CultureInfo.CurrentCulture.Name);
            }
            catch (Exception ex)
            {
                result = new OperationResultListOfPlayersInGame
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };
                Log.Error(ex.Message);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<bool> ExpelPlayerAsync(ExpelPlayerDto expelPlayer, int idPlayer, GameDto game)
        {
            OpenConnection();
            bool result = false;
            try
            {
                result = await client.ExpelPlayerAsAdminAsync(expelPlayer, idPlayer, game);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<bool> VoteExpelPlayerAsync(ExpelPlayerDto expelPlayer, int idPlayer, GameDto game)
        {
            OpenConnection();
            bool result = false;
            try
            {
                result = await client.VoteExpelPlayerAsync(expelPlayer, idPlayer, game);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }


        public async Task ThrowDiceAsync(PlayerGameplayDto playerGameplayDto, GameDto gameClientDto, int diceValue)
        {
            OpenConnection();
            try
            {
                await client.ThrowDiceAsync(playerGameplayDto, gameClientDto, diceValue);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
           
        }

        public async Task<bool> GiveNextTurn(PlayerGameplayDto playerGameplayDto, GameDto gameClientDto)
        {
            OpenConnection();
            bool result = false;
            try
            {
                result = await client.NextTurnAyncAsync(playerGameplayDto, gameClientDto);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return result;
        }

        public void ExitGame(PlayerGameplayDto playerGameplayDto, GameDto gameClientDto)
        {
            OpenConnection();
            try
            {
                client.QuitGamePlay(playerGameplayDto, gameClientDto);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}