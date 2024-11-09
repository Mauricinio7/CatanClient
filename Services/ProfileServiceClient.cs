using CatanClient.ProfileService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Globalization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Services
{
    internal class ProfileServiceClient : IProfileServiceClient
    {
        private NetTcpBinding GetTcpBinding()
        {
            return new NetTcpBinding
            {
                Security = { Mode = SecurityMode.None },
                MaxBufferSize = 10485760,
                MaxReceivedMessageSize = 10485760,
                OpenTimeout = TimeSpan.FromMinutes(1),
                CloseTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(2),
                ReceiveTimeout = TimeSpan.FromMinutes(10)
            };
        }

        private void SafeClose(IClientChannel client, ChannelFactory channelFactory)
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
            NetTcpBinding binding = GetTcpBinding();
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
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public OperationResultProfileDto UploadImage(ProfileDto profile, byte[] image)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultProfileDto result;

            try
            {
                result = client.UploadProfilePicture(profile, image);
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
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public OperationResultPictureDto GetImage(ProfileDto profile)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultPictureDto result;

            try
            {
                result = client.GetProfilePicture(profile);
            }
            catch (Exception ex)
            {
                result = new OperationResultPictureDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message,
                };
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public bool SendFriendRequest(string playerName, ProfileDto profile)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.SendFriendRequest(playerName, profile);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public OperationResultProfileListDto GetFriendRequestList(ProfileDto profile)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultProfileListDto result;

            try
            {
                result = client.GetPendingFriendRequests(profile);
            }
            catch (Exception ex)
            {
                result = new OperationResultProfileListDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message,
                };
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public OperationResultProfileListDto GetFriendList(ProfileDto profile)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultProfileListDto result;

            try
            {
                result = client.GetFriendsList(profile);
            }
            catch (Exception ex)
            {
                result = new OperationResultProfileListDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message,
                };
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public bool AcceptFriendRequest(string playerName, ProfileDto profile)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.AcceptFriendRequest(playerName, profile);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public bool RejectFriendRequest(string playerName, ProfileDto profile)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.RejectFriendRequest(playerName, profile);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public bool DeleteFriend(string playerName, ProfileDto profile)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.DeleteFriendProfile(playerName, profile);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public bool InviteFriendToGame(string playerName, ProfileDto profile, string accessKey)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = client.InviteFriendsToGame(playerName, profile, accessKey);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }

        public OperationResultPictureDto GetFriendImage(ProfileDto profile)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultPictureDto result;

            try
            {
                result = client.GetFriendsPicture(profile, CultureInfo.CurrentCulture.Name);
            }
            catch (Exception ex)
            {
                result = new OperationResultPictureDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message,
                };
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            return result;
        }
    }
}