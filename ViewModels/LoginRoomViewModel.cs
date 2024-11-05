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

            LoginRoomCommand = new RelayCommand(ExecuteLoginRoom);
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



        private void ExecuteLoginRoom(object parameter)
        { 
            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;

            ProfileDto profile = new ProfileDto
            {
                Name = profileDto.Name,
                Id = profileDto.Id,
                CurrentSessionID = profileDto.CurrentSessionID,
                PreferredLanguage = CultureInfo.CurrentCulture.Name
            };

            if (!String.IsNullOrWhiteSpace(roomCode))
            {
                OperationResultGameDto result;

                //if (profile.IsRegistered)
                //{
                    result = serviceManager.GameServiceClient.JoinRoomClient(roomCode, profile);
                //}
                //else  
                //{
                //    GuestAccountDto guest = new GuestAccountDto 
                //    {
                //        Name = profile.Name,
                //        PreferredLanguage = profile.PreferredLanguage,
                //        Id = profile.Id
                //    };

                    //result = serviceManager.GameServiceClient.JoinRoomAsGuestClient(roomCode, guest);
                //}
                    
                     

                if (result.IsSuccess == true)
                {
                    GameDto game = result.GameDto;
                    Mediator.Notify(Utilities.SHOWGAMELOBBY, game);
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
