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
            BasicHttpBinding binding = new BasicHttpBinding();
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
            BasicHttpBinding binding = new BasicHttpBinding();
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

        public bool LeftRoomClient(GameDto game, ProfileDto profile)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
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
