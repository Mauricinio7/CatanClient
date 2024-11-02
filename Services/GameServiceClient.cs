﻿using CatanClient.GameService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Services
{
    public class GameServiceClient : IGameServiceClient
    {
        public OperationResultGameDto CreateRoomClient(GameDto game, ProfileDto profile)
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
            ChannelFactory<IGameEndPoint> channelFactory = new ChannelFactory<IGameEndPoint>(binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
            OperationResultGameDto result;

            try
            {
                result = client.CreateGame(game, profile);
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
            return result;
        }

        public OperationResultGameDto JoinRoomClient(string code, ProfileDto profile)
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
            ChannelFactory<IGameEndPoint> channelFactory = new ChannelFactory<IGameEndPoint>(binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
            OperationResultGameDto result;

            try
            {
                result = client.JoinGame(code, profile);
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
            return result;
        }

        public OperationResultGameDto JoinRoomAsGuestClient(string code, GuestAccountDto profile)
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
            ChannelFactory<IGameEndPoint> channelFactory = new ChannelFactory<IGameEndPoint>(binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
            OperationResultGameDto result;

            try
            {
                result = client.JoinGameAsaGuest(code, profile);
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
            return result;
        }

        public bool LeftRoomClient(GameDto game, ProfileDto profile)
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
            ChannelFactory<IGameEndPoint> channelFactory = new ChannelFactory<IGameEndPoint>(binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.QuitGame(game, profile).IsSuccess;
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
            return result;
        }
    }
}