using CatanClient.ProfileService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Globalization;
using System.ServiceModel;


namespace CatanClient.Services
{
    internal class ProfileServiceClient : IProfileServiceClient
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

        public OperationResultProfileDto ChangeName(ProfileDto profile, string newName)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultProfileDto result = new OperationResultProfileDto { IsSuccess = false };

            profile.Name = newName;

            try
            {
                result = client.ChangeProfileName(profile);
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
            }
            return result;
        }

        public OperationResultProfileDto UploadImage(ProfileDto profile, byte[] image)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultProfileDto result = new OperationResultProfileDto { IsSuccess = false };

            try
            {
                result = client.UploadProfilePicture(profile, image);
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
            }
            return result;
        }

        public OperationResultPictureDto GetImage(ProfileDto profile)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultPictureDto result = new OperationResultPictureDto { IsSuccess = false };

            try
            {
                result = client.GetProfilePicture(profile);
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
            }
            return result;
        }

        public OperationResultFriendRequestDto SendFriendRequest(string playerName, ProfileDto profile)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultFriendRequestDto result = new OperationResultFriendRequestDto { IsSuccess = false, StatusSendFriendRequest = EnumSendFriendRequest.ErrorSaving};

            try
            {
                result = client.SendFriendRequest(playerName, profile);
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
            }
            return result;
        }

        public OperationResultProfileListDto GetFriendRequestList(ProfileDto profile)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultProfileListDto result = new OperationResultProfileListDto { IsSuccess = false };

            try
            {
                result = client.GetPendingFriendRequests(profile);
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
            }
            return result;
        }

        public OperationResultProfileListDto GetFriendList(ProfileDto profile)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultProfileListDto result = new OperationResultProfileListDto { IsSuccess = false };

            try
            {
                result = client.GetFriendsList(profile);
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
            }
            return result;
        }

        public bool AcceptFriendRequest(string playerName, ProfileDto profile)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.AcceptFriendRequest(playerName, profile);
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
            }
            return result;
        }

        public bool RejectFriendRequest(string playerName, ProfileDto profile)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.RejectFriendRequest(playerName, profile);
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
            }
            return result;
        }

        public bool DeleteFriend(string playerName, ProfileDto profile)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.DeleteFriendProfile(playerName, profile);
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
            }
            return result;
        }

        public bool InviteFriendToGame(string playerName, ProfileDto profile, string accessKey)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.InviteFriendsToGame(playerName, profile, accessKey);
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
            }
            return result;
        }

        public OperationResultPictureDto GetFriendImage(ProfileDto profile)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultPictureDto result = new OperationResultPictureDto { IsSuccess = false };

            try
            {
                result = client.GetFriendsPicture(profile, CultureInfo.CurrentCulture.Name);
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
            }
            return result;
        }
    }
}