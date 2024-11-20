using Autofac;
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
        private ChatService.GameDto game;
        private AccountService.ProfileDto profile;
        private int remainingTimeInSeconds;
        private DispatcherTimer countdownTimer;
        private bool isJoiningGame;


        public string RoomName => game.Name;
        public string AccessCode => game.AccessKey;
        public List<ProfileDto> OnlinePlayers { get; set; } = new List<ProfileDto>();
        public List<GuestAccountDto> OnlinePlayersGuest { get; set; } = new List<GuestAccountDto>();
        public ObservableCollection<PlayerInRoomCardViewModel> OnlinePlayersList { get; set; } = new ObservableCollection<PlayerInRoomCardViewModel>();
        public ICommand SendMessageCommand { get; }
        public ICommand LeftRoomCommand { get; }
        public ICommand KickPlayerCommand { get; }
        public ICommand ShowInviteFriendCommand { get; }
        public ICommand ToggleReadyCommand { get; }
        private readonly ServiceManager serviceManager;
        private bool isReady;
        public ObservableCollection<ChatMessage> Messages { get; set; }

        public string TimeText => isJoiningGame
        ? Utilities.LabelJoiningGame(CultureInfo.CurrentCulture.Name)
        : remainingTimeInSeconds > 0
            ? $"Tiempo restante: {TimeSpan.FromSeconds(remainingTimeInSeconds):mm\\:ss}"
            : Utilities.LabelWaitingPlayers(CultureInfo.CurrentCulture.Name);

        public bool IsReady
        {
            get => isReady;
            set
            {
                isReady = value;
                OnPropertyChanged(nameof(IsReady));
                OnPropertyChanged(nameof(ReadyButtonText)); 
            }
        }

        public string ReadyButtonText => IsReady ? Utilities.GlobalNoReady(CultureInfo.CurrentCulture.Name) : Utilities.GlobalReady(CultureInfo.CurrentCulture.Name);

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

            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage { Content = game.Name + " ID: " + game.AccessKey, Name = Utilities.SYSTEM_NAME,IsUserMessage = false }
            };

            profile =  serviceManager.ProfileSingleton.Profile;

            Mediator.Register(Utilities.RECIVE_MESSAGE, OnReceiveMessage);
            Mediator.Register(Utilities.LOAD_PLAYER_LIST, LoadPlayerList);

            this.serviceManager.ChatServiceClient.JoinChatClient(game, profile.Name);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);
            LeftRoomCommand = new AsyncRelayCommand(ExecuteLeftRoomAsync);
            KickPlayerCommand = new RelayCommand(_ => ExecuteShowKickPlayer(), _ => CanKickPlayer());
            ShowInviteFriendCommand = new RelayCommand(_ => ExecuteShowInviteFriend(), _ => CanInviteFriends());
            ToggleReadyCommand = new AsyncRelayCommand(ToggleReady);

            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            countdownTimer.Tick += CountdownTimer_Tick;
            Mediator.Register(Utilities.UPDATE_TIME, SetCountdownTime);
            Mediator.Register(Utilities.GET_GAME_FOR_SCREEN, ShowGameScreen);


            UpdateCanExecuteCommands();
        }


        private async Task ToggleReady()
        {
            IsReady = !IsReady;
            if (IsReady)
            {
                await ExecuteIsReady();
            }
            else
            {
                await ExecuteIsNotReady();
            }
        }

        private async Task ExecuteIsReady()
        {
            bool result = await serviceManager.GameServiceClient.StartGameAsync(AccountUtilities.CastAccountProfileToPlayerGameplay(profile), AccountUtilities.CastChatGameToGameServiceGame(game));
        }

        private async Task ExecuteIsNotReady()
        {
            bool result = await serviceManager.GameServiceClient.CancelStartGameAsync(AccountUtilities.CastAccountProfileToPlayerGameplay(profile), AccountUtilities.CastChatGameToGameServiceGame(game));
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

            result = serviceManager.GameServiceClient.GetPlayerList(AccountUtilities.CastChatGameToGameServiceGame(game));

            if (result.IsSuccess)
            {
                OnlinePlayersList.Clear();
                OnlinePlayers = result.ProfileDtos.ToList();  
                OnlinePlayersGuest = result.GuestAccountDtos.ToList();

                if (OnlinePlayers.Count > 0 && OnlinePlayers != null)
                {
                    foreach (var profileDto in OnlinePlayers)
                    {
                        OnlinePlayersList.Add(App.Container.Resolve<PlayerInRoomCardViewModel>(
                            new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto))));
                    }
                }

                if(OnlinePlayersGuest.Count > 0 && OnlinePlayersGuest != null)
                {
                    foreach (var guestProfileDto in OnlinePlayersGuest)
                    {
                        ProfileDto profileDto = new ProfileDto();
                        profileDto.Id = guestProfileDto.Id;
                        profileDto.Name = guestProfileDto.Name;
                        profileDto.IsRegistered = false;
                        profileDto.PictureVersion = 0;
                        profileDto.PreferredLanguage = CultureInfo.CurrentCulture.Name;

                        OnlinePlayersList.Add(App.Container.Resolve<PlayerInRoomCardViewModel>(
                            new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(profileDto))));
                    }
                }
                
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
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
            var kickPlayerWindow = new KickPlayerWindow(game);
            kickPlayerWindow.ShowDialog();
        }

        private bool CanKickPlayer()
        {
            return profile.IsRegistered && game.IdAdminGame == profile.Id;
        }

        internal void ExecuteSendMessage()
        {
            serviceManager.ChatServiceClient.SendMessageToServer(game, profile.Name, NewMessage);
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


