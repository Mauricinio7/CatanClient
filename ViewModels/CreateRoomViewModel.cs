using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Serilog;
using System.ServiceModel;
using CatanClient.GameService;
using System.Xml.XPath;
using CatanClient.Singleton;

namespace CatanClient.ViewModels
{
    internal class CreateRoomViewModel : ViewModelBase
    {
        private string selectedOption;
        private string roomName;

        public ObservableCollection<string> OptionsList { get; set; }

        public string SelectedOption
        {
            get { return selectedOption; }
            set
            {
                if (selectedOption != value)
                {
                    selectedOption = value;
                    OnPropertyChanged(nameof(SelectedOption));
                }
            }
        }

        public string RoomName
        {
            get => roomName;
            set
            {
                roomName = value;
                OnPropertyChanged(nameof(roomName));
            }
        }


        public ICommand CreateRoomCommand { get; }
        private readonly ServiceManager serviceManager;

        public CreateRoomViewModel(ServiceManager serviceManager)
        {
            OptionsList = new ObservableCollection<string>
            {
                "2",
                "3",
                "4",
            };

            CreateRoomCommand = new RelayCommand(ExecuteCreateRoom);
            this.serviceManager = serviceManager;
        }


        private void ExecuteCreateRoom(object parameter)
        {

            GameDto gameDto = new GameDto
            {
                MaxNumberPlayers = int.Parse(SelectedOption),
                Name = RoomName

            };

            var profileDto = serviceManager.ProfileSingleton.Profile;

            ProfileDto profile = new ProfileDto
            {
                Name = profileDto.Name,
                Id = profileDto.Id,
                PicturePath = profileDto.PicturePath,
                PreferredLanguage = CultureInfo.CurrentCulture.Name, //TODO quit hardcode and do it whit actual culture               
            };

            OperationResultGameDto result = serviceManager.GameServiceClient.CreateRoomClient(gameDto, profile);

            GameDto game = result.GameDto;

            MessageBox.Show(game.Name);

            Mediator.Notify(Utilities.SHOWGAMELOBBY, game);
            
        }

        
    }
}
