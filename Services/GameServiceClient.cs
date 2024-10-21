using CatanClient.GameService;
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
    public static class GameServiceClient
    {
        public static OperationResultGameDto CreateRoomClient(GameDto game, ProfileDto profile)
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(Utilities.IP_GAME_SERVICE);
            var channelFactory = new ChannelFactory<IGameEndPoint>(binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
            OperationResultGameDto result = null;

            try
            {
                result = client.CreateGame(game, profile);

                MessageBox.Show(result.IsSuccess.ToString());
            }
            catch (Exception ex) //TODO do a good exception 
            {
                Log.Information(ex.Message);
                MessageBox.Show($"{ex.Message}");
            }
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

        public static OperationResultGameDto JoinRoomClient(string code, ProfileDto profile)
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(Utilities.IP_GAME_SERVICE);
            var channelFactory = new ChannelFactory<IGameEndPoint>(binding, endpoint);
            IGameEndPoint client = channelFactory.CreateChannel();
            OperationResultGameDto result = null;

            try
            {
                MessageBox.Show(profile.Name);


                result = client.JoinGame(code, profile);

                MessageBox.Show(result.IsSuccess.ToString());

                return result;
            }
            catch (Exception ex) //TODO do a good exception 
            {
                Log.Information(ex.Message);
                return result;
            }
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
        }
    }
}
