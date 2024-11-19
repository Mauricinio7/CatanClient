using Autofac;
using CatanClient.ChatService;
using CatanClient.Commands;
using CatanClient.Controls;
using CatanClient.Gameplay.Helpers;
using CatanClient.Gameplay.Views;
using CatanClient.GameService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace CatanClient.ViewModels
{
    internal class GameFrameViewModel : ViewModelBase
    {
        private string newMessage;
        private ChatService.GameDto game;
        private AccountService.ProfileDto profile;
        private int remainingTimeInSeconds;
        private bool turn;

        private DispatcherTimer countdownTimer;
        public ICommand ShowTradeWindowCommand { get; }
        public ICommand RollDiceCommand { get; }
        public ICommand NextTurnCommand { get; }
        public ICommand HideTradeControlCommand { get; }
        public ICommand ShowTradeControlCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICollectionView PlayersView { get; set; }

        private readonly ServiceManager serviceManager;
        public ObservableCollection<ChatMessage> Messages { get; set; }
        public ObservableCollection<PlayerInGameCardViewModel> OnlinePlayersList { get; set; } = new ObservableCollection<PlayerInGameCardViewModel>();
        public ObservableCollection<Resource> ResourcesToRequest { get; set; }
        public ObservableCollection<Resource> ResourcesToOffer { get; set; }
        public ObservableCollection<Resource> FilteredResourcesToRequest =>
           new ObservableCollection<Resource>(ResourcesToRequest.Where(r => r.Quantity > 0));

        public ObservableCollection<Resource> FilteredResourcesToOffer =>
            new ObservableCollection<Resource>(ResourcesToOffer.Where(r => r.Quantity > 0));

        public string TimeText => remainingTimeInSeconds > 0
       ? $"Tiempo restante: {TimeSpan.FromSeconds(remainingTimeInSeconds):mm\\:ss}"
       : "Asignando turno...";


        private bool isTradeGridVisible;

        public bool Turn
        {
            get => turn;
            set
            {
                if (turn != value)
                {
                    turn = value;
                    OnPropertyChanged(nameof(Turn)); 

                    ((RelayCommand)RollDiceCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)NextTurnCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)ShowTradeWindowCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsTradeGridVisible
        {
            get => isTradeGridVisible;
            set
            {
                isTradeGridVisible = value;
                OnPropertyChanged(nameof(IsTradeGridVisible));
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




        public GameFrameViewModel(ChatService.GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;

            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage { Content = "El juego ha iniciado", Name = Utilities.SYSTEM_NAME,IsUserMessage = false }
            };

            profile = serviceManager.ProfileSingleton.Profile;

            Mediator.Register(Utilities.RECIVE_MESSAGE_GAME, OnReceiveMessage);
            ShowTradeWindowCommand = new RelayCommand(_ => ExecuteShowTradeWindow(), _ => CanPlayTurn());
            RollDiceCommand = new RelayCommand(_ => ExecuteRollDiceAsync(), _ => CanPlayTurn()); 
            NextTurnCommand = new RelayCommand(_ => ExecuteNextTurn(), _ => CanPlayTurn()); 
            HideTradeControlCommand = new RelayCommand(ExecuteHideTradeControl);
            ShowTradeControlCommand = new RelayCommand(ExecuteShowTradeControl);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);

            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            countdownTimer.Tick += CountdownTimer_Tick;
            Mediator.Register(Utilities.UPDATE_TIME_GAME, SetCountdownTime);


            LoadResources(); //TODO quit this

            Mediator.Register(Utilities.LOAD_GAME_PLAYER_LIST, LoadPlayerList);

            IsTradeGridVisible = true;
            UpdateTradeWindow();
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

        private void LoadResources()
        {
            ResourcesToRequest = new ObservableCollection<Resource>
            {
                new Resource { Name = "Lunar Stone", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/LunarStone.png", Quantity = 1 },
                new Resource { Name = "Tritonium", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/TritoniumWood.png" },
                new Resource { Name = "Alpha Nanofibers", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/AlphaNanofibers.png", Quantity = 5 },
                new Resource { Name = "Epsilon Biomass", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/EpsilonGrain.png", Quantity = 2 },
                new Resource { Name = "GRX-810", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/GRX-810Stone.png", Quantity = 4 }
            };
            ResourcesToOffer = new ObservableCollection<Resource>
            {
                new Resource { Name = "Lunar Stone", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/LunarStone.png", Quantity = 1 },
                new Resource { Name = "Tritonium", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/TritoniumWood.png" },
                new Resource { Name = "Alpha Nanofibers", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/AlphaNanofibers.png", Quantity = 0 },
                new Resource { Name = "Epsilon Biomass", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/EpsilonGrain.png", Quantity = 2 },
                new Resource { Name = "GRX-810", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/GRX-810Stone.png", Quantity = 0 }
            };
        }

        internal void UpdateTradeWindow()
        {
            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(1000);
                IsTradeGridVisible = false;
            });
        }

        private bool CanPlayTurn()
        {
            return turn == true;
        }

        public void ExecuteShowTradeWindow() 
        {
            TradeWindow tradeWindow = new TradeWindow();
            tradeWindow.ShowDialog();
        }

        public void ExecuteRollDiceAsync() 
        {
            PlayerGameplayDto playerGameplay = new PlayerGameplayDto();
            playerGameplay.CurrentSession = profile.CurrentSessionID;
            playerGameplay.isRegistered = profile.IsRegistered;
            playerGameplay.Id = profile.Id.Value;

            bool result = serviceManager.GameServiceClient.ThrowDice(playerGameplay, AccountUtilities.CastChatGameToGameServiceGame(game));

            MessageBox.Show(result.ToString());
        }

        public void ExecuteNextTurn() 
        {
            MessageBox.Show("Siguiente turno");
        }

        public void ExecuteHideTradeControl()
        {
            IsTradeGridVisible = false;
        }

        public void ExecuteShowTradeControl()
        {
            IsTradeGridVisible = true;
        }

        internal void LoadPlayerList(object playerList)
        {
            List<PlayerTurnStatusDto> playersTurnStatus = (playerList as PlayerTurnStatusDto[])?.ToList();

            if (playersTurnStatus == null)
            {
                Utilities.ShowMessageDataBaseUnableToLoad();
            }
            else
            {
                OnlinePlayersList.Clear();

                foreach (PlayerTurnStatusDto player in playersTurnStatus)
                {
                    if (player.Profile != null)
                    {
                        if(profile.Id == player.Profile.Id)
                        {
                            Turn = player.IsTurn;
                        }
                        OnlinePlayersList.Add(App.Container.Resolve<PlayerInGameCardViewModel>(
                            new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(player.Profile)),
                            new NamedParameter(Utilities.POINTS, player.Points),
                            new NamedParameter(Utilities.TURN, player.IsTurn)
                            ));
                    }
                    else
                    {
                        ProfileService.ProfileDto profileDto = new ProfileService.ProfileDto
                        {
                            Id = player.GuestAccount.Id,
                            Name = player.GuestAccount.Name,
                            IsRegistered = false,
                            PictureVersion = 0,
                            PreferredLanguage = CultureInfo.CurrentCulture.Name
                        };
                        if (profile.Id == profileDto.Id)
                        {
                            Turn = player.IsTurn;
                        }
                        OnlinePlayersList.Add(App.Container.Resolve<PlayerInGameCardViewModel>(
                            new NamedParameter(Utilities.PROFILE, profileDto),
                            new NamedParameter(Utilities.POINTS, player.Points),
                            new NamedParameter(Utilities.TURN, player.IsTurn)
                            ));
                    }
                }

                PlayersView = CollectionViewSource.GetDefaultView(OnlinePlayersList);
                PlayersView.SortDescriptions.Add(new SortDescription(nameof(PlayerInGameCardViewModel.Turn), ListSortDirection.Descending));
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

        internal void ExecuteSendMessage()
        {
            serviceManager.ChatServiceClient.SendMessageToServer(game, profile.Name, NewMessage);
        }



    }
}
