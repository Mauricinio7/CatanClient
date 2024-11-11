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

        public async Task<OperationResultProfileDto> IsValidAuthenticationAsync(AccountDto account)
        {
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultProfileDto result;

            try
            {
                result = await client.LogInAsync(account);
            }
            catch (Exception ex)
            {
                result = new OperationResultProfileDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message,
                    AunthenticationStatus = EnumAuthenticationStatus.ServerNotFound
                };
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

            NetTcpBinding binding = GetTcpBinding();
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

            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultCreateAccountDto result;

            try
            {
                result = await client.CreateAccountAsync(account);
            }
            catch (Exception ex)
            {
                result = new OperationResultCreateAccountDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };
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

            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultChangeRegisterEmailOrPhone result;

            try
            {
                result = await client.ChangeEmailOrPhoneAsync(account);
            }
            catch (Exception ex)
            {
                result = new OperationResultChangeRegisterEmailOrPhone
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };
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

            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result;

            try
            {
                result = await client.ChangePasswordAsync(account);
            }
            catch (Exception ex)
            {
                result = new OperationResultDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };
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

            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultChangeRegisterEmailOrPhone result;

            try
            {
                result = await client.SendVerificationCodeToChangeEmailOrPhoneAsync(account);
            }
            catch (Exception ex)
            {
                result = new OperationResultChangeRegisterEmailOrPhone
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };
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

            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result;

            try
            {
                result = await client.SendVerificationCodeToChangePasswordAsync(account);
            }
            catch (Exception ex)
            {
                result = new OperationResultDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
                };
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
            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultAccountDto result;

            try
            {
                result = client.ConsultAccounProfileInformationAsync(profile).Result;
            }
            catch (Exception ex)
            {
                result = new OperationResultAccountDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message,
                };
                Log.Information(ex.Message);
                MessageBox.Show(Utilities.MessageServerLostConnection(CultureInfo.CurrentCulture.Name), Utilities.TittleServerLostConnection(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
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

            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            bool flag = false;

            try
            {

                flag = await client.ResendVerificationCodeAsync(account.Email);
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

            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = await client.NeedHelpProblemWithPasswordAsync(email);
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

            NetTcpBinding binding = GetTcpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_ACCOUNT_SERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            bool result = false;

            try
            {
                result = await client.ChangeForgotPasswordAsync(email, newPassword, verificationCode);
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