using CatanClient.AccountService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.Singleton
{
    public interface IProfileSingleton
    {
        ProfileDto Profile { get; }
        void SetProfile(ProfileDto profile);
    }
}
