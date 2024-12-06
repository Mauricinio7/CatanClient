using CatanClient.AccountService;
using CatanClient.Commands;
using CatanClient.GameService;
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
using static CatanClient.ViewModels.ScoreboardViewModel;

namespace CatanClient.ViewModels
{
    internal class MainMenuViewModel : ViewModelBase
    {
        public ICommand ShowConfigureProfileCommand { get; }
        public ICommand ShowScoreBoardCommand { get; }
        private readonly ServiceManager serviceManager;
        private AccountService.ProfileDto profile;
       

        public MainMenuViewModel(ServiceManager serviceManager)
        {
            ShowConfigureProfileCommand = new RelayCommand(OnShowConfigureProfile);
            ShowScoreBoardCommand = new RelayCommand(OnShowScoreBoard);
            this.serviceManager = serviceManager;

            profile = serviceManager.ProfileSingleton.Profile;


        }

        private void OnShowScoreBoard(object parameter)
        {

            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                OperationResultListScoreGame result = await serviceManager.GameServiceClient.GetScoreboardFriends(AccountUtilities.CastAccountProfileToGameService(profile));
                Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);

                if (result.IsSuccess)
                {
                    Mediator.Notify(Utilities.SHOW_SCORE_FRAME, null);
                }
                else
                {
                    Utilities.ShowMessgeServerLost();
                }
            });
            
            
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
