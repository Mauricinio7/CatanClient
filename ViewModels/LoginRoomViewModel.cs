﻿using CatanClient.Commands;
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

            if (!string.IsNullOrWhiteSpace(roomCode))
            {
                if (!AccountUtilities.IsValidLength(roomCode))
                {
                    Utilities.ShowMessageInvalidFileds();
                }
                else
                {
                    OperationResultGameDto result;

                    try
                    {
                        if (profile.IsRegistered)
                        {
                            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                            result = await serviceManager.GameServiceClient.JoinRoomClientAsync(roomCode, AccountUtilities.CastAccountProfileToGameService(profile));
                        }
                        else
                        {
                            GuestAccountDto guest = new GuestAccountDto
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
                            GameDto game = result.GameDto;
                            Mediator.Notify(Utilities.SHOW_GAME_LOBBY, game);
                        }
                        else
                        {
                            MessageBox.Show(Utilities.MessageGameNotFound(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    catch (CommunicationException)
                    {
                        Utilities.ShowMessgeServerLost();
                    }
                }
            }
            else
            {
                Utilities.ShowMessageEmptyFields();
            }
        }
    }
}
