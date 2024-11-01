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
    }
}
