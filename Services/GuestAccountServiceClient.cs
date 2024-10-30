using CatanClient.GuestAccountService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Services
{
    internal class GuestAccountServiceClient : IGuestAccountServiceClient
    {
        public OperationResultGuestAccountDto LoginAsGuest(string language)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_GUEST_ACCOUNT_SERVICE);
            ChannelFactory<IGuestAccountEndpoint> channelFactory = new ChannelFactory<IGuestAccountEndpoint>(binding, endpoint);
            IGuestAccountEndpoint client = channelFactory.CreateChannel();
            OperationResultGuestAccountDto result;

            try
            {
                result = client.CreateGuestAccount(language);

                MessageBox.Show(result.IsSuccess.ToString());
                MessageBox.Show(result.MessageResponse);
            }
            catch (Exception ex)
            {
                result = new OperationResultGuestAccountDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };

                Log.Error(ex.Message);
                MessageBox.Show(result.MessageResponse);
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
