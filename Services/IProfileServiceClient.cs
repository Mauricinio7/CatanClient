using CatanClient.ProfileService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.Services
{
    internal interface IProfileServiceClient
    {
        OperationResultProfileDto ChangeName(ProfileDto profile, string newName);
        OperationResultProfileDto UploadImage(ProfileDto profile, byte[] image);
        OperationResultPictureDto GetImage(ProfileDto profile);
        bool SendFriendRequest(string playerName, ProfileDto profile);
        OperationResultProfileListDto GetFriendRequestList(ProfileDto profile);
        bool AcceptFriendRequest(string playerName, ProfileDto profile);
        bool RejectFriendRequest(string playerName, ProfileDto profile);
        OperationResultProfileListDto GetFriendList(ProfileDto profile);
        bool InviteFriendToGame(string playerName, ProfileDto profile, string accessKey);
        bool DeleteFriend(string playerName, ProfileDto profile);

        OperationResultPictureDto GetFriendImage(ProfileDto profile);
    }
}
