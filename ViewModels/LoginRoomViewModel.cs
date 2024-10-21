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

        public LoginRoomViewModel()
        {

            LoginRoomCommand = new RelayCommand(ExecuteLoginRoom);
        }



        private void ExecuteLoginRoom(object parameter)
        {

            MessageBox.Show(roomCode);

            var profileDto = ProfileSingleton.Instance.Profile;

            ProfileDto profile = new ProfileDto
            {
                Name = profileDto.Name,
                Id = profileDto.Id,
                PicturePath = profileDto.PicturePath,
                PreferredLanguage = CultureInfo.CurrentCulture.Name
            };


            OperationResultGameDto result = GameServiceClient.JoinRoomClient(roomCode, profile); //TODO quit hardcode

            GameDto game = result.GameDto;

            if(game == null)
            {
                MessageBox.Show("Me manda el game null el maldito");
                return;
            }

            Mediator.Notify(Utilities.SHOWGAMELOBBY, game);

        }

       
    }
}
