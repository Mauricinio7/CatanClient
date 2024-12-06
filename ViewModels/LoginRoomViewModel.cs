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
        private readonly ServiceManager serviceManager;
        public ICommand LoginRoomCommand { get; }
        public ICommand ExitLoginRoomCommand { get; }
        


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
            AccountService.ProfileDto profile = serviceManager.ProfileSingleton.Profile;

            if (string.IsNullOrWhiteSpace(roomCode))
            {
                Utilities.ShowMessageEmptyFields();
                return;
            }

            if (!AccountUtilities.IsValidLength(roomCode))
            {
                MessageBox.Show(Utilities.MessageTooLargeInput(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                var result = profile.IsRegistered
                    ? await JoinRoomAsRegisteredUserAsync(profile)
                    : await JoinRoomAsGuestUserAsync(profile);

                HandleJoinRoomResult(result);
            }
            catch (CommunicationException)
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        private async Task<OperationResultGameDto> JoinRoomAsRegisteredUserAsync(AccountService.ProfileDto profile)
        {
            return await serviceManager.GameServiceClient.JoinRoomClientAsync(
                roomCode, AccountUtilities.CastAccountProfileToGameService(profile));
        }

        private async Task<OperationResultGameDto> JoinRoomAsGuestUserAsync(AccountService.ProfileDto profile)
        {
            var guest = new GuestAccountDto
            {
                Name = profile.Name,
                PreferredLanguage = profile.PreferredLanguage,
                Id = profile.Id
            };

            return await serviceManager.GameServiceClient.JoinRoomAsGuestClientAsync(roomCode, guest);
        }

        private static void HandleJoinRoomResult(OperationResultGameDto result)
        {
            if (result.IsSuccess)
            {
                Mediator.Notify(Utilities.SHOW_GAME_LOBBY, result.GameDto);
            }
            else
            {
                MessageBox.Show(
                    Utilities.MessageGameNotFound(CultureInfo.CurrentCulture.Name),
                    Utilities.TittleFail(CultureInfo.CurrentCulture.Name),
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}
