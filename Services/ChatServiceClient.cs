﻿using CatanClient.Callbacks;
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
    public class ChatServiceClient : IChatServiceClient
    {
        private static ChatCallback callback = new ChatCallback();
        public void JoinChatClient(ChatService.GameDto game, string namePlayer)
        {
            InstanceContext instanceContext = new InstanceContext(callback);

            NetTcpBinding netTcpBinding = new NetTcpBinding();
            netTcpBinding.Security.Mode = SecurityMode.None;

            EndpointAddress endpointAddress = new EndpointAddress(Utilities.IP_CHAT_SERVICE);

            DuplexChannelFactory<IChatServiceEndpoint> factory = new DuplexChannelFactory<IChatServiceEndpoint>(instanceContext, netTcpBinding, endpointAddress);

            IChatServiceEndpoint chatService = factory.CreateChannel();

            chatService.JoinChat(game, namePlayer);

        }

        public void LeftChatClient(ChatService.GameDto game, string namePlayer)
        {
            InstanceContext instanceContext = new InstanceContext(callback);

            NetTcpBinding netTcpBinding = new NetTcpBinding();
            netTcpBinding.Security.Mode = SecurityMode.None;

            EndpointAddress endpointAddress = new EndpointAddress(Utilities.IP_CHAT_SERVICE);

            DuplexChannelFactory<IChatServiceEndpoint> factory = new DuplexChannelFactory<IChatServiceEndpoint>(instanceContext, netTcpBinding, endpointAddress);

            IChatServiceEndpoint chatService = factory.CreateChannel();

            chatService.LeaveChat(game, namePlayer);
        }

        public void SendMessageToServer(ChatService.GameDto game, string namePlayer, string message)
        {
            InstanceContext instanceContext = new InstanceContext(callback);

            NetTcpBinding netTcpBinding = new NetTcpBinding();
            netTcpBinding.Security.Mode = SecurityMode.None;

            EndpointAddress endpointAddress = new EndpointAddress(Utilities.IP_CHAT_SERVICE);

            DuplexChannelFactory<IChatServiceEndpoint> factory = new DuplexChannelFactory<IChatServiceEndpoint>(instanceContext, netTcpBinding, endpointAddress);

            IChatServiceEndpoint chatService = factory.CreateChannel();

            chatService.SendMessageToChat(game, namePlayer, message);

        }
    }
}
