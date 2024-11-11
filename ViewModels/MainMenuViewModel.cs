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
       

        public MainMenuViewModel(ServiceManager serviceManager)
        {

            ShowConfigureProfileCommand = new RelayCommand(OnShowConfigureProfile);
            this.serviceManager = serviceManager;
        }


        private void OnShowConfigureProfile(object parameter)
        {
            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profileDto.PreferredLanguage = CultureInfo.CurrentCulture.Name;

            OperationResultAccountDto result;
            

            result = serviceManager.AccountServiceClient.GetAccount(profileDto);

            if (result.IsSuccess)
            {
                AccountDto account = result.AccountDto;

                if(account != null)
                {
                    Mediator.Notify(Utilities.SHOW_CONFIGURE_PROFILE, account);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        

    }
}
