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
                if (roomName != value)  
                {
                    roomName = value;
                    OnPropertyChanged(nameof(RoomName));  
                }
            }
        }


        public ICommand CreateRoomCommand { get; }
        public ICommand ExitCreateRoomCommand { get; }
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
            ExitCreateRoomCommand = new RelayCommand(ExecuteExitCreateRoom);
            this.serviceManager = serviceManager;
        }

        private void ExecuteExitCreateRoom(object parameter)
        {
            AccountService.ProfileDto profile = serviceManager.ProfileSingleton.Profile;

            if (profile.IsRegistered)
            {
                Mediator.Notify(Utilities.BACK_FROM_CREATE_ROOM, profile);
            }
            else
            {

            }
        }

        private void ExecuteCreateRoom(object parameter)
        {
            if(!String.IsNullOrEmpty(RoomName) && !String.IsNullOrEmpty(SelectedOption))
            {
                GameDto gameDto = new GameDto
                {
                    MaxNumberPlayers = int.Parse(SelectedOption),
                    Name = RoomName

                };

                AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;

                ProfileDto profile = new ProfileDto
                {
                    Name = profileDto.Name,
                    Id = profileDto.Id,
                    PicturePath = profileDto.PicturePath,
                    PreferredLanguage = CultureInfo.CurrentCulture.Name,               
                };
                OperationResultGameDto result = serviceManager.GameServiceClient.CreateRoomClient(gameDto, profile);

                if (result.IsSuccess)
                {
                    GameDto game = result.GameDto;
                    Mediator.Notify(Utilities.SHOWGAMELOBBY, game);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}
