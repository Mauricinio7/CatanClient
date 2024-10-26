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
                Log.Information(ex.Message);
                MessageBox.Show(Utilities.MessageServerLostConnection(CultureInfo.CurrentCulture.Name), Utilities.TittleServerLostConnection(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
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
                MessageBox.Show(result.IsSuccess + Utilities.STRING_CHAR_SPACE + result.MessageResponse);

                flag = result.IsSuccess;

            }
            catch (Exception ex)
            {
                Log.Information(ex.Message);
                MessageBox.Show(Utilities.MessageServerLostConnection(CultureInfo.CurrentCulture.Name), Utilities.TittleServerLostConnection(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return flag;
        }

        public OperationResultChangeRegisterEmailOrPhone ChangeEmail(AccountDto account)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultChangeRegisterEmailOrPhone result;

            try
            {
                result = client.ChangeEmailOrPhone(account);

                MessageBox.Show(result.IsSuccess.ToString());

            }
            catch (Exception ex)
            {
                result = new OperationResultChangeRegisterEmailOrPhone
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
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
        public OperationResultChangeRegisterEmailOrPhone ConfirmEmail(AccountDto account)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IPACCOUNTSERVICE);
            ChannelFactory<IAccountEndPoint> channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultChangeRegisterEmailOrPhone result;

            try
            {
                result = client.SendVerificationCodeToChangeEmailOrPhone(account);

                MessageBox.Show(result.IsSuccess.ToString());

            }
            catch (Exception ex)
            {
                result = new OperationResultChangeRegisterEmailOrPhone
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message
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
