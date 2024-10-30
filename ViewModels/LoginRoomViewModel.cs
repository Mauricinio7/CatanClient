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
        private readonly ServiceManager serviceManager;

        public LoginRoomViewModel(ServiceManager serviceManager)
        {

            LoginRoomCommand = new RelayCommand(ExecuteLoginRoom);
            this.serviceManager = serviceManager;
        }



        private void ExecuteLoginRoom(object parameter)
        { 
            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;

            ProfileDto profile = new ProfileDto
            {
                Name = profileDto.Name,
                Id = profileDto.Id,
                PicturePath = profileDto.PicturePath,
                PreferredLanguage = CultureInfo.CurrentCulture.Name
            }; 

            if(!String.IsNullOrWhiteSpace(roomCode))
            {
                OperationResultGameDto result = serviceManager.GameServiceClient.JoinRoomClient(roomCode, profile);

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
