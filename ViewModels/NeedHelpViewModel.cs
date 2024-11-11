using CatanClient.AccountService;
using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CatanClient.ViewModels
{
    internal class NeedHelpViewModel : ViewModelBase
    {
        private string email;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public ICommand SendCodeCommand { get; }

        private readonly ServiceManager serviceManager;

        public NeedHelpViewModel(ServiceManager serviceManager)
        {
            SendCodeCommand = new AsyncRelayCommand(ExecuteSendCodeAsync);
            this.serviceManager = serviceManager;
        }

        private async Task ExecuteSendCodeAsync(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
            bool status = await serviceManager.AccountServiceClient.SendNeedHelpCode(Email);

            if (status)
            {
                MessageBox.Show(Utilities.MessageSuccesSendVerificationCode(CultureInfo.CurrentCulture.Name), Utilities.TitleVerifyAccount(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                Mediator.Notify(Utilities.SHOW_CHANGE_FORGOT_PASSWORD, Email);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        
    }
}
