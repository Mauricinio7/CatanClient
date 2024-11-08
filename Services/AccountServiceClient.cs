using CatanClient.AccountService;
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
    public class AccountServiceClient : IAccountServiceClient
    {
        public async Task<OperationResultProfileDto> IsValidAuthenticationAsync(AccountDto account)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
                
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<bool> VerifyUserAccountAsync(AccountDto account, string token)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
                
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return flag;
        }

        public async Task<OperationResultCreateAccountDto> CreateAccountInServerAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
                
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultChangeRegisterEmailOrPhone> ChangeEmailOrPhoneAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultDto> ChangePasswordAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
                
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultChangeRegisterEmailOrPhone> ConfirmEmailOrPhoneAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
                
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public async Task<OperationResultDto> ConfirmPasswordAsync(AccountDto account)
        {
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
                
            }
            Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            return result;
        }

        public OperationResultAccountDto GetAccount(ProfileDto profile)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultAccountDto result;

            try
            {
                result = client.ConsultAccounProfileInformationAsync(profile).Result;

                //MessageBox.Show(result.AccountDto.PhoneNumber);
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
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }

    }
}
