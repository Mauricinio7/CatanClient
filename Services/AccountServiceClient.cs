using CatanClient.AccountService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Globalization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Services
{
    public class AccountServiceClient : IAccountServiceClient
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

        public async Task<OperationResultProfileDto> IsValidAuthenticationAsync(AccountDto account)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultProfileDto result = new OperationResultProfileDto
            {
                IsSuccess = false,
                AunthenticationStatus = EnumAuthenticationStatus.ServerNotFound
            };

            try
            {
                result = await client.LogInAsync(account);
            }
            catch (EndpointNotFoundException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<bool> VerifyUserAccountAsync(AccountDto account, string token)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            bool flag = false;

            try
            {
                account.Token = token;
                OperationResultDto result = await client.VerifyAccountAsync(account);
                flag = result.IsSuccess;
            }
            catch (EndpointNotFoundException ex)
            {
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return flag;
        }

        public async Task<OperationResultCreateAccountDto> CreateAccountInServerAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultCreateAccountDto result;

            try
            {
                result = await client.CreateAccountAsync(account);
            }
            catch (EndpointNotFoundException ex)
            {
                result = new OperationResultCreateAccountDto { IsSuccess = false, MessageResponse = ex.Message };
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                result = new OperationResultCreateAccountDto { IsSuccess = false, MessageResponse = ex.Message };
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                result = new OperationResultCreateAccountDto { IsSuccess = false, MessageResponse = ex.Message };
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                result = new OperationResultCreateAccountDto { IsSuccess = false, MessageResponse = ex.Message };
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultChangeRegisterEmailOrPhone> ChangeEmailOrPhoneAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultChangeRegisterEmailOrPhone result = new OperationResultChangeRegisterEmailOrPhone { IsSuccess = false };

            try
            {
                result = await client.ChangeEmailOrPhoneAsync(account);
            }
            catch (EndpointNotFoundException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultDto> ChangePasswordAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result = new OperationResultDto { IsSuccess = false };

            try
            {
                result = await client.ChangePasswordAsync(account);
            }
            catch (EndpointNotFoundException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultChangeRegisterEmailOrPhone> ConfirmEmailOrPhoneAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultChangeRegisterEmailOrPhone result = new OperationResultChangeRegisterEmailOrPhone { IsSuccess = false};

            try
            {
                result = await client.SendVerificationCodeToChangeEmailOrPhoneAsync(account);
            }
            catch (EndpointNotFoundException ex)
            {
                        
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultDto> ConfirmPasswordAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result = new OperationResultDto { IsSuccess = false };

            try
            {
                result = await client.SendVerificationCodeToChangePasswordAsync(account);
            }
            catch (EndpointNotFoundException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public OperationResultAccountDto GetAccount(ProfileDto profile)
        {
            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultAccountDto result = new OperationResultAccountDto { IsSuccess = false };

            try
            {
                result = client.ConsultAccounProfileInformationAsync(profile).Result;
            }
            catch (EndpointNotFoundException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                result.MessageResponse = ex.Message;
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }

            return result;
        }

        public async Task<bool> ResendCodeAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            bool flag = false;

            try
            {
                flag = await client.ResendVerificationCodeAsync(account.Email);
            }
            catch (EndpointNotFoundException ex)
            {
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return flag;
        }

        public async Task<bool> SendNeedHelpCode(string email)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = await client.NeedHelpProblemWithPasswordAsync(email);
            }
            catch (EndpointNotFoundException ex)
            {
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<bool> ChangeForotPassword(string email, string newPassword, string verificationCode)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            NetTcpBinding binding = ConnectionUtilities.GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = await client.ChangeForgotPasswordAsync(email, newPassword, verificationCode);
            }
            catch (EndpointNotFoundException ex)
            {
                Log.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.Message);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                SafeClose((IClientChannel)client, channelFactory);
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

    }
}