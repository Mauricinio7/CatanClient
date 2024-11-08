using CatanClient.Callbacks;
using CatanClient.GameService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace CatanClient.Services
{
    public class GameServiceClient : IGameServiceClient
    {
        public async Task<OperationResultGameDto> CreateRoomClientAsync(GameDto game, ProfileDto profile)
        {
            NetTcpBinding binding = new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None },
                MaxBufferSize = 10485760,
                MaxReceivedMessageSize = 10485760,
                OpenTimeout = TimeSpan.FromMinutes(1),
                CloseTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(2),
                ReceiveTimeout = TimeSpan.FromMinutes(10)
            };
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_GAME_SERVICE);
            InstanceContext callbackInstance = new InstanceContext(new GameCallback());
            DuplexChannelFactory<IGameEndPoint> channelFactory = new DuplexChannelFactory<IGameEndPoint>(callbackInstance, binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
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
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultGameDto> JoinRoomClientAsync(string code, ProfileDto profile)
        {
            NetTcpBinding binding = new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None },
                MaxBufferSize = 10485760,
                MaxReceivedMessageSize = 10485760,
                OpenTimeout = TimeSpan.FromMinutes(1),
                CloseTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(2),
                ReceiveTimeout = TimeSpan.FromMinutes(10)
            };
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_GAME_SERVICE);
            InstanceContext callbackInstance = new InstanceContext(new GameCallback());
            DuplexChannelFactory<IGameEndPoint> channelFactory = new DuplexChannelFactory<IGameEndPoint>(callbackInstance, binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
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
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultGameDto> JoinRoomAsGuestClientAsync(string code, GuestAccountDto profile)
        {
            NetTcpBinding binding = new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None },
                MaxBufferSize = 10485760,
                MaxReceivedMessageSize = 10485760,
                OpenTimeout = TimeSpan.FromMinutes(1),
                CloseTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(2),
                ReceiveTimeout = TimeSpan.FromMinutes(10)
            };
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_GAME_SERVICE);
            InstanceContext callbackInstance = new InstanceContext(new GameCallback());
            DuplexChannelFactory<IGameEndPoint> channelFactory = new DuplexChannelFactory<IGameEndPoint>(callbackInstance, binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
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
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<bool> LeftRoomClientAsync(GameDto game, ProfileDto profile)
        {
            NetTcpBinding binding = new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None },
                MaxBufferSize = 10485760,
                MaxReceivedMessageSize = 10485760,
                OpenTimeout = TimeSpan.FromMinutes(1),
                CloseTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(2),
                ReceiveTimeout = TimeSpan.FromMinutes(10)
            };
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_GAME_SERVICE);
            InstanceContext callbackInstance = new InstanceContext(new GameCallback());
            DuplexChannelFactory<IGameEndPoint> channelFactory = new DuplexChannelFactory<IGameEndPoint>(callbackInstance, binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public OperationResultListOfPlayersInGame GetPlayerList(GameDto game)
        {
            NetTcpBinding binding = new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None },
                MaxBufferSize = 10485760,
                MaxReceivedMessageSize = 10485760,
                OpenTimeout = TimeSpan.FromMinutes(1),
                CloseTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(2),
                ReceiveTimeout = TimeSpan.FromMinutes(10)
            };
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_GAME_SERVICE);
            InstanceContext callbackInstance = new InstanceContext(new GameCallback());
            DuplexChannelFactory<IGameEndPoint> channelFactory = new DuplexChannelFactory<IGameEndPoint>(callbackInstance, binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
            OperationResultListOfPlayersInGame result;

            try
            {
                result = client.GetAllPlayersInGame(game, CultureInfo.CurrentCulture.Name);
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
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

        public async Task<bool> ExpelPlayerAsync(ExpelPlayerDto expelPlayer, int idPlayer, GameDto game)
        {
            InstanceContext callbackInstance = new InstanceContext(new GameCallback());
            NetTcpBinding netTcpBinding = new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None }
            };

            EndpointAddress endpointAddress = new EndpointAddress(Utilities.IP_GAME_SERVICE);
            DuplexChannelFactory<IGameEndPoint> factory = new DuplexChannelFactory<IGameEndPoint>(callbackInstance, netTcpBinding, endpointAddress);
            IGameEndPoint client = factory.CreateChannel();
            bool result;

            try
            {
                result = await client.ExpelPlayerAsAdminAsync(expelPlayer, idPlayer, game);
                MessageBox.Show(result.ToString());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                result = false;
            }
            finally
            {
                ((IClientChannel)client).Close();
                factory.Close();
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }
    }

    

    
}