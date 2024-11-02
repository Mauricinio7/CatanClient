using CatanClient.AccountService;
using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class MainMenuViewModel : ViewModelBase
    {
        public ICommand ShowConfigureProfileCommand { get; }
        private readonly ServiceManager serviceManager;
        public ICommand KickPlayerCommand { get; } //TODO quit

        public MainMenuViewModel(ServiceManager serviceManager)
        {

            ShowConfigureProfileCommand = new RelayCommand(OnShowConfigureProfile);
            KickPlayerCommand = new RelayCommand(ExecuteShowKickPlayer); //TODO quit
            this.serviceManager = serviceManager;
        }

        internal void ExecuteShowKickPlayer() //TODO quit
        {
            var kickPlayerWindow = new KickPlayerWindow();

            kickPlayerWindow.ShowDialog();
        }

        private void OnShowConfigureProfile(object parameter)
        {
            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profileDto.PreferredLanguage = CultureInfo.CurrentCulture.Name;

            OperationResultAccountDto result;
            

            result = GetAccount(profileDto);

            if (result.IsSuccess)
            {
                AccountDto account = result.AccountDto;

                if(account != null)
                {
                    Mediator.Notify(Utilities.SHOWCONFIGUREPROFILE, account);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
