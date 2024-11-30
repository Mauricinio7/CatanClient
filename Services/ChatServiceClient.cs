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
        private readonly InstanceContext callbackInstance;

        public ChatServiceClient()
        {
            callbackInstance = new InstanceContext(new ChatCallback());
        }

        private void OpenConnection()
        {
            if (client != null) return;

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

            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_CHAT_SERVICE);
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
                    Log.Error(ex, ex.Source);
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
            catch (EndpointNotFoundException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
            }
        }

        public void LeftChatClient(GameDto game, string namePlayer)
        {
            OpenConnection();
            try
            {
                client.LeaveChatAsync(game, namePlayer);
            }
            catch (EndpointNotFoundException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
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
            catch (EndpointNotFoundException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
            }
        }
    }
}