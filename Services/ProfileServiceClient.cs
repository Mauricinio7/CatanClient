using CatanClient.ProfileService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Services
{
    internal class ProfileServiceClient : IProfileServiceClient
    {
        public OperationResultProfileDto ChangeName(ProfileDto profile, string newName)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultProfileDto result;

            profile.Name = newName;

            try
            {
                result = client.ChangeProfileName(profile);
            }
            catch (Exception ex)
            {
                result = new OperationResultProfileDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message,
                };

                Log.Error(ex.Message);
            }
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }
    }
}
