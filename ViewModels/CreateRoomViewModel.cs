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
        private readonly ServiceManager serviceManager;
        public ICommand CreateRoomCommand { get; }
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


        

        public CreateRoomViewModel(ServiceManager serviceManager)
        {
            InicializateOptionsList();
            CreateRoomCommand = new AsyncRelayCommand(ExecuteCreateRoomAsync);
            this.serviceManager = serviceManager;
        }

        private void InicializateOptionsList()
        {
            OptionsList = new ObservableCollection<string>
            {
                "2",
                "3",
                "4",
            };
        }


        private async Task ExecuteCreateRoomAsync(object parameter)
        {
            if (!string.IsNullOrEmpty(RoomName) && !string.IsNullOrEmpty(SelectedOption))
            {
                if (!AccountUtilities.IsValidLength(RoomName))
                {
                    Utilities.ShowMessageInvalidFileds();
                }
                else
                {

                    GameDto gameDto = new GameDto
                    {
                        MaxNumberPlayers = int.Parse(SelectedOption),
                        Name = RoomName
                    };

                    AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;

                    Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);

                    OperationResultGameDto result = await serviceManager.GameServiceClient.CreateRoomClientAsync(gameDto, AccountUtilities.CastAccountProfileToGameService(profileDto));

                    if (result.IsSuccess)
                    {
                        GameDto game = result.GameDto;
                        Mediator.Notify(Utilities.SHOW_GAME_LOBBY, game);
                    }
                    else
                    {
                        Utilities.ShowMessgeServerLost();
                    }
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
