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

        public OperationResultProfileDto IsValidAuthentication(AccountDto account)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultProfileDto result;

            try
            {
                result = client.LogInAsync(account).Result;

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
            return result;
        }

        public bool VerifyUserAccount(AccountDto account, string token)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result;
            bool flag = false;

            try
            {
                account.Token = token;
                result = client.VerifyAccountAsync(account).Result;
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
            return flag;
        }
        public async Task<bool> CreateAccountInServerAsync(AccountDto account)
        {

            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result;
            bool flag = false;

            try
            {

                result = await client.CreateAccountAsync(account);

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
            return flag;
        }

        public OperationResultChangeRegisterEmailOrPhone ChangeEmailOrPhone(AccountDto account)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultChangeRegisterEmailOrPhone result;

            try
            {
                result = client.ChangeEmailOrPhone(account);

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
            return result;
        }

        public OperationResultDto ChangePassword(AccountDto account)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result;

            try
            {
                result = client.ChangePassword(account);

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
            return result;
        }

        public OperationResultChangeRegisterEmailOrPhone ConfirmEmailOrPhone(AccountDto account)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultChangeRegisterEmailOrPhone result;

            try
            {
                result = client.SendVerificationCodeToChangeEmailOrPhone(account);

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
            return result;
        }

        public OperationResultDto ConfirmPassword(AccountDto account)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultDto result;

            try
            {
                result = client.SendVerificationCodeToChangePassword(account);

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
            return result;
        }
    }
}
