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
    public interface IChatServiceClient
    {
        void JoinChatClient(ChatService.GameDto game, ProfileDto profile);

        void SendMessageToServer(ChatService.GameDto game, ProfileDto profile, string message);
    }
}
