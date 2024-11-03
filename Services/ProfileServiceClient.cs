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
using System.Windows.Input;

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

        public OperationResultProfileDto UploadImage(ProfileDto profile, byte[] image)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

        public OperationResultPictureDto GetImage(ProfileDto profile)
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760, 
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = 10485760, 
                    MaxStringContentLength = 8192,
                    MaxBytesPerRead = 4096,
                    MaxDepth = 32,
                    MaxNameTableCharCount = 16384
                }
            };

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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

        public bool SendFriendRequest(string playerName, ProfileDto profile)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

        public OperationResultProfileListDto GetFriendRequestList(ProfileDto profile)
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = 10485760,
                    MaxStringContentLength = 8192,
                    MaxBytesPerRead = 4096,
                    MaxDepth = 32,
                    MaxNameTableCharCount = 16384
                }
            };
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

        public OperationResultProfileListDto GetFriendList(ProfileDto profile)
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = 10485760,
                    MaxStringContentLength = 8192,
                    MaxBytesPerRead = 4096,
                    MaxDepth = 32,
                    MaxNameTableCharCount = 16384
                }
            };
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

        public bool AcceptFriendRequest(string playerName, ProfileDto profile)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

        public bool RejectFriendRequest(string playerName, ProfileDto profile)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

    }
}
