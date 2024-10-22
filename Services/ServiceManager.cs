using CatanClient.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.Services
{
    internal class ServiceManager
    {
        private readonly Lazy<IAccountServiceClient> accountServiceClient;
        private readonly Lazy<IGameServiceClient> gameServiceClient;
        private readonly Lazy<IChatServiceClient> chatServiceClient;
        private readonly Lazy<IProfileSingleton> profileSingleton;

        public ServiceManager(Lazy<IAccountServiceClient> accountServiceClient, Lazy<IGameServiceClient> gameServiceClient, Lazy<IChatServiceClient> chatServiceClient, Lazy<IProfileSingleton> profileSingleton)
        {
            this.accountServiceClient = accountServiceClient;
            this.gameServiceClient = gameServiceClient;
            this.chatServiceClient = chatServiceClient;
            this.profileSingleton = profileSingleton;
        }


        public IAccountServiceClient AccountServiceClient => accountServiceClient.Value;
        public IGameServiceClient GameServiceClient => gameServiceClient.Value;
        public IChatServiceClient ChatServiceClient => chatServiceClient.Value;
        public IProfileSingleton ProfileSingleton => profileSingleton.Value; 
    }
}
