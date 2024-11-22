using CatanClient.Callbacks;
using CatanClient.ChatService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.ServiceModel;
using System.Windows;

namespace CatanClient.Services
{
    public class ChatServiceClient : IChatServiceClient
    {
        private DuplexChannelFactory<IChatServiceEndpoint> channelFactory;
        private IChatServiceEndpoint client;
        private InstanceContext callbackInstance;

        public ChatServiceClient()
        {
            callbackInstance = new InstanceContext(new ChatCallback());
        }

        private void OpenConnection()
        {
            if (client != null) return;

            var binding = new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None },
                MaxBufferSize = 10485760,
                MaxReceivedMessageSize = 10485760,
                OpenTimeout = TimeSpan.FromMinutes(1),
                CloseTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(2),
                ReceiveTimeout = TimeSpan.FromMinutes(10)
            };

            var endpoint = new EndpointAddress(Utilities.IP_CHAT_SERVICE);
            channelFactory = new DuplexChannelFactory<IChatServiceEndpoint>(callbackInstance, binding, endpoint);
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

        public void JoinChatClient(GameDto game, string namePlayer)
        {
            OpenConnection();
            try
            {
                client.JoinChatAsync(game, namePlayer);
            }
            catch (Exception ex)
            {
                Log.Error($"Error al unirse al chat: {ex.Message}");
            }
        }

        public void LeftChatClient(GameDto game, string namePlayer)
        {
            OpenConnection();
            try
            {
                client.LeaveChatAsync(game, namePlayer);
            }
            catch (Exception ex)
            {
                Log.Error($"Error al salir del chat: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        public void SendMessageToServer(GameDto game, string namePlayer, string message)
        {
            try
            {
                client.SendMessageToChatAsync(game, namePlayer, message);
            }
            catch (Exception ex)
            {
                Log.Error($"Error al enviar mensaje al chat: {ex.Message}");
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}