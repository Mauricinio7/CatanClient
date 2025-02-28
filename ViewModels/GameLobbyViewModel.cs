﻿using Autofac;
using CatanClient.Callbacks;
using CatanClient.ChatService;
using CatanClient.Commands;
using CatanClient.Controls;
using CatanClient.GameService;
using CatanClient.Services;
using CatanClient.Singleton;
using CatanClient.UIHelpers;
using CatanClient.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CatanClient.ViewModels
{
    internal class GameLobbyViewModel : ViewModelBase
    {
        private string newMessage;
        private readonly ChatService.GameDto game;
        private readonly AccountService.ProfileDto profile;
        private int remainingTimeInSeconds;
        private DispatcherTimer countdownTimer;
        private readonly bool isJoiningGame;
        private readonly ServiceManager serviceManager;
        private bool isReady;
        public string RoomName => game.Name;
        public string AccessCode => game.AccessKey;
        public List<ProfileDto> OnlinePlayers { get; set; } = new List<ProfileDto>();
        public List<GuestAccountDto> OnlinePlayersGuest { get; set; } = new List<GuestAccountDto>();
        public ObservableCollection<PlayerInRoomCardViewModel> OnlinePlayersList { get; set; } = new ObservableCollection<PlayerInRoomCardViewModel>();
        public ObservableCollection<ChatMessage> Messages { get; set; }
        public ICommand SendMessageCommand { get; set; }
        public ICommand LeftRoomCommand { get; set; }
        public ICommand KickPlayerCommand { get; set; }
        public ICommand ShowInviteFriendCommand { get; set; }
        public ICommand ReadyCommand { get; set; }



        public string TimeText
        {
            get
            {
                if (isJoiningGame)
                {
                    return Utilities.LabelJoiningGame(CultureInfo.CurrentCulture.Name);
                }

                if (remainingTimeInSeconds > 0)
                {
                    string formattedTime = TimeSpan.FromSeconds(remainingTimeInSeconds).ToString(Utilities.TIME_FORMAT);
                    return Utilities.GetTimeRemainingText(CultureInfo.CurrentCulture.Name, formattedTime);
                }

                return Utilities.LabelWaitingPlayers(CultureInfo.CurrentCulture.Name);
            }
        }

        public bool IsReady
        {
            get => isReady;
            set
            {
                isReady = value;
                OnPropertyChanged(nameof(IsReady));
                (ReadyCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged(); 
            }
        }

        public string NewMessage
        {
            get { return newMessage; }
            set
            {
                newMessage = value;
                OnPropertyChanged(nameof(NewMessage));
            }
        }

        public GameLobbyViewModel(ChatService.GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;
            profile =  serviceManager.ProfileSingleton.Profile;
            SendInitMessage();
            JoinChat();
            InicializateAsyncCommands();
            InicializateCommands();
            MediatorRegister();
            InicializateCountdownTimer();
            UpdateCanExecuteCommands();
        }

        private void InicializateCountdownTimer()
        {
            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void InicializateCommands()
        {
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);
            KickPlayerCommand = new RelayCommand(_ => ExecuteShowKickPlayer(), _ => CanKickPlayer());
            ShowInviteFriendCommand = new RelayCommand(_ => ExecuteShowInviteFriend(), _ => CanInviteFriends());
        }

        private void InicializateAsyncCommands()
        {
            LeftRoomCommand = new AsyncRelayCommand(ExecuteLeftRoomAsync);
            ReadyCommand = new AsyncRelayCommand(_ => Ready(), _ => CanExecuteReady());
        }

        private void SendInitMessage()
        {
            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage { Content = game.Name + " ID: " + game.AccessKey, Name = Utilities.SYSTEM_NAME,IsUserMessage = false }
            };
        }

        private void JoinChat()
        {
            try
            {
                serviceManager.ChatServiceClient.JoinChatClient(game, profile.Name);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Utilities.ShowMessgeServerLost();
                AccountUtilities.RestartGame();
            }
            
        }

        private void UpdateGameAdmin(object idAdmin)
        {
            if (!(idAdmin is int IdAdmin))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                game.IdAdminGame = IdAdmin;
                UpdateCanExecuteCommands();
            }
        }

        private void MediatorRegister()
        {
            Mediator.Register(Utilities.RECIVE_MESSAGE, OnReceiveMessage);
            Mediator.Register(Utilities.LOAD_PLAYER_LIST, LoadPlayerList);
            Mediator.Register(Utilities.UPDATE_TIME, SetCountdownTime);
            Mediator.Register(Utilities.GET_GAME_FOR_SCREEN, ShowGameScreen);
            Mediator.Register(Utilities.UPDATE_GAME_ADMIN, args => UpdateGameAdmin(args));
        }

        private bool CanExecuteReady()
        {
            return !IsReady; 
        }
        private async Task Ready()
        {
            IsReady = true;
            await ExecuteIsReady();
            await Task.Delay(2000);
            LoadPlayerList(null);
        }

        private async Task ExecuteIsReady()
        {
            bool result = await serviceManager.GameServiceClient.StartGameAsync(AccountUtilities.CastAccountProfileToPlayerGameplay(profile), AccountUtilities.CastChatGameToGameServiceGame(game));
            if (!result)
            {
                Utilities.ShowMessgeServerLost();
            }
        }


        public void SetCountdownTime(object timeInSeconds)
        {
            remainingTimeInSeconds = (int)timeInSeconds;
            OnPropertyChanged(nameof(TimeText));
            StartCountdown();
        }

        private void StartCountdown()
        {
            countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (remainingTimeInSeconds > 0)
            {
                remainingTimeInSeconds--;
                OnPropertyChanged(nameof(TimeText));
            }
            else
            {
                countdownTimer.Stop();
            }
        }

        private void UpdateCanExecuteCommands()
        {
            (KickPlayerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ShowInviteFriendCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        internal void LoadPlayerList(object parameter)
        {
            OperationResultListOfPlayersInGame result;

            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                result = await serviceManager.GameServiceClient.GetPlayerListAsync(AccountUtilities.CastChatGameToGameServiceGame(game));

                if (result.IsSuccess)
                {
                    UpdateOnlinePlayersLists(result);
                }
                else
                {
                    Utilities.ShowMessgeServerLost();
                }
            });
        }

        private void UpdateOnlinePlayersLists(OperationResultListOfPlayersInGame result)
        {
            OnlinePlayersList.Clear();
            OnlinePlayers = result.ProfileDtos.ToList();
            OnlinePlayersGuest = result.GuestAccountDtos.ToList();

            AddRegisteredPlayersToList(OnlinePlayers);
            AddGuestPlayersToList(OnlinePlayersGuest);
        }

        private void AddRegisteredPlayersToList(IEnumerable<ProfileDto> registeredPlayers)
        {
            foreach (var profileDto in registeredPlayers)
            {
                OnlinePlayersList.Add(App.Container.Resolve<PlayerInRoomCardViewModel>(
                    new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto))));
            }
        }

        private void AddGuestPlayersToList(IEnumerable<GuestAccountDto> guestPlayers)
        {
            foreach (var guestProfileDto in guestPlayers)
            {
                var profileDto = MapGuestToProfile(guestProfileDto);
                OnlinePlayersList.Add(App.Container.Resolve<PlayerInRoomCardViewModel>(
                    new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto))));
            }
        }

        private ProfileDto MapGuestToProfile(GuestAccountDto guestProfileDto)
        {
            return new ProfileDto
            {
                Id = guestProfileDto.Id,
                Name = guestProfileDto.Name,
                IsRegistered = false,
                PictureVersion = 0,
                PreferredLanguage = CultureInfo.CurrentCulture.Name,
                isReadyToPlay = guestProfileDto.isReadyToPlay
            };
        }

        internal void OnReceiveMessage(object message)
        {
            ChatMessage chatMessage = message as ChatMessage;
            if (chatMessage != null)
            {
                Messages.Add(chatMessage);
            }
        }

        internal void ExecuteShowInviteFriend()
        {
            Mediator.Notify(Utilities.SHOW_INVITE_FRIENDS, game.AccessKey);
        }

        private bool CanInviteFriends()
        {
            return profile.IsRegistered;
        }
        internal void ExecuteShowKickPlayer()
        {
            KickPlayerWindow kickPlayerWindow = new KickPlayerWindow(game);
            kickPlayerWindow.ShowDialog();
        }

        private bool CanKickPlayer()
        {
            return profile.IsRegistered && game.IdAdminGame == profile.Id;
        }

        internal void ExecuteSendMessage()
        {
            if(string.IsNullOrWhiteSpace(NewMessage))
            {
                NewMessage = string.Empty;
            }
            else
            {
                serviceManager.ChatServiceClient.SendMessageToServer(game, profile.Name, NewMessage);
                NewMessage = string.Empty;
            }
        }

        internal async Task ExecuteLeftRoomAsync()
        {
            serviceManager.ChatServiceClient.LeftChatClient(game, profile.Name);

            GameService.GameDto gameRoom = new GameService.GameDto
            {
                Name = game.Name,
                Id = game.Id,
                AccessKey = game.AccessKey,
                MaxNumberPlayers = game.MaxNumberPlayers
            };
            Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
            if (profile.IsRegistered)
            {
                await serviceManager.GameServiceClient.LeftRoomClientAsync(gameRoom, AccountUtilities.CastAccountProfileToGameService(profile));
            }
            else
            {
                 await serviceManager.GameServiceClient.LeftRoomGuestClientAsync(gameRoom, AccountUtilities.CastAccountProfileToGuestAccount(profile));
            }

            LoadPlayerList(null);

            AccountUtilities.RestartGame();
        }

        private void ShowGameScreen(object args)
        {
                Mediator.Notify(Utilities.SHOW_GAME_SCREEN, game);
        }


    }
    


}


