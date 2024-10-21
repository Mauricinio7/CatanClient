using CatanClient.Callbacks;
using CatanClient.ChatService;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.Services
{
    public static class ChatServiceClient
    {
        private static ChatCallback callback = new ChatCallback();
        public static void JoinChatClient(ChatService.GameDto game, ProfileDto profile)
        {
            var instanceContext = new InstanceContext(callback);

            var netTcpBinding = new NetTcpBinding();
            netTcpBinding.Security.Mode = SecurityMode.None;

            var endpointAddress = new EndpointAddress(Utilities.IP_CHAT_SERVICE);

            var factory = new DuplexChannelFactory<IChatServiceEndpoint>(instanceContext, netTcpBinding, endpointAddress);

            var chatService = factory.CreateChannel();

            chatService.JoinChat(game, profile);

        }

        public static void SendMessageToServer(ChatService.GameDto game, ProfileDto profile, string message)
        {
            var instanceContext = new InstanceContext(callback);

            var netTcpBinding = new NetTcpBinding();
            netTcpBinding.Security.Mode = SecurityMode.None;

            var endpointAddress = new EndpointAddress(Utilities.IP_CHAT_SERVICE);

            var factory = new DuplexChannelFactory<IChatServiceEndpoint>(instanceContext, netTcpBinding, endpointAddress);

            var chatService = factory.CreateChannel();

            chatService.SendMessageToChat(game, profile, message);

        }
    }
}
