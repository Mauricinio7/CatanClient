using CatanClient.Commands;
using CatanClient.GameService;
using CatanClient.Singleton;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CatanClient.Services;
using System.Globalization;

namespace CatanClient.ViewModels
{
    internal class LoginRoomViewModel : ViewModelBase
    {
        private string roomCode;


        public string RoomCode
        {
            get => roomCode;
            set
            {
                if (roomCode != value)
                {
                    roomCode = value;
                    OnPropertyChanged(nameof(RoomCode)); 
                }
            }
        }


        public ICommand LoginRoomCommand { get; }
        public ICommand ExitLoginRoomCommand { get; }
        private readonly ServiceManager serviceManager;

        public LoginRoomViewModel(ServiceManager serviceManager)
        {

            LoginRoomCommand = new AsyncRelayCommand(ExecuteLoginRoomAsync);
            ExitLoginRoomCommand = new RelayCommand(ExecuteExitLoginRoom);
            this.serviceManager = serviceManager;
        }

        private void ExecuteExitLoginRoom(object parameter)
        {
            AccountService.ProfileDto profile = serviceManager.ProfileSingleton.Profile;

            if (profile.IsRegistered)
            {
                Mediator.Notify(Utilities.BACK_TO_MAIN_MENU_ROOM, null);
            }
            else
            {
                Mediator.Notify(Utilities.BACK_TO_GUEST_MAIN_MENU_ROOM, null);
            }
        }



        private async Task ExecuteLoginRoomAsync(object parameter)
        {
            var profile = serviceManager.ProfileSingleton.Profile;

            if (!string.IsNullOrWhiteSpace(roomCode))
            {
                OperationResultGameDto result;

                if (profile.IsRegistered)
                {
                    Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                    result = await serviceManager.GameServiceClient.JoinRoomClientAsync(roomCode, AccountUtilities.CastAccountProfileToGameService(profile));
                }
                else
                {
                    var guest = new GuestAccountDto
                    {
                        Name = profile.Name,
                        PreferredLanguage = profile.PreferredLanguage,
                        Id = profile.Id
                    };

                    Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                    result = await serviceManager.GameServiceClient.JoinRoomAsGuestClientAsync(roomCode, guest);
                }

                if (result.IsSuccess)
                {
                    var game = result.GameDto;
                    Mediator.Notify(Utilities.SHOW_GAME_LOBBY, game);
                }
                else
                {
                    MessageBox.Show(result.MessageResponse);
                    MessageBox.Show(Utilities.MessageGameNotFound(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
