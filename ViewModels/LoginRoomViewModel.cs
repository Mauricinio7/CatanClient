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
                roomCode = value;
                OnPropertyChanged(nameof(roomCode));
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

            MessageBox.Show(roomCode);

            var profileDto = serviceManager.ProfileSingleton.Profile;

            ProfileDto profile = new ProfileDto
            {
                Name = profileDto.Name,
                Id = profileDto.Id,
                PicturePath = profileDto.PicturePath,
                PreferredLanguage = CultureInfo.CurrentCulture.Name
            };


            OperationResultGameDto result = serviceManager.GameServiceClient.JoinRoomClient(roomCode, profile); //TODO quit hardcode

            GameDto game = result.GameDto;

            if(game == null)
            {
                return;
            }

            Mediator.Notify(Utilities.SHOWGAMELOBBY, game);

        }

       
    }
}
