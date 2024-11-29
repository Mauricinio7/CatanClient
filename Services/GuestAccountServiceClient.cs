using CatanClient.GuestAccountService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Services
{
    internal class GuestAccountServiceClient : IGuestAccountServiceClient
    {

        private static void SafeClose(IClientChannel client, ChannelFactory channelFactory)
        {
            if (client != null)
            {
                if (client.State == CommunicationState.Faulted)
                    client.Abort();
                else
                    client.Close();
            }

            if (channelFactory != null)
            {
                if (channelFactory.State == CommunicationState.Faulted)
                    channelFactory.Abort();
                else
                    channelFactory.Close();
            }
        }

        public async Task<OperationResultGuestAccountDto> LoginAsGuestAsync(string language)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_GUEST_ACCOUNT_SERVICE);
            ChannelFactory<IGuestAccountEndpoint> channelFactory = new ChannelFactory<IGuestAccountEndpoint>(binding, endpoint);
            IGuestAccountEndpoint client = channelFactory.CreateChannel();
            OperationResultGuestAccountDto result = new OperationResultGuestAccountDto { IsSuccess = false };

            try
            {
                result = await client.CreateGuestAccountAsync(language);
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
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, ex.Source);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
                Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            }
            return result;
        }
    }
}