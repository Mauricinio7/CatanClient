﻿using Autofac;
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
        private PlayerGameplayDto playerGameplay;

        private DispatcherTimer countdownTimer;
        public ICommand ShowTradeWindowCommand { get; }
        public ICommand RollDiceCommand { get; }
        public ICommand NextTurnCommand { get; }
        public ICommand HideTradeControlCommand { get; }
        public ICommand ShowTradeControlCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand VoteKickCommand { get; }
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

            
            ShowTradeWindowCommand = new RelayCommand(_ => ExecuteShowTradeWindow(), _ => CanPlayTurn());
            RollDiceCommand = new RelayCommand(_ => ExecuteRollDiceAsync(), _ => CanPlayTurn()); 
            NextTurnCommand = new RelayCommand(_ => ExecuteNextTurn(), _ => CanPlayTurn()); 
            HideTradeControlCommand = new RelayCommand(ExecuteHideTradeControl);
            ShowTradeControlCommand = new RelayCommand(ExecuteShowTradeControl);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);
            ExitCommand = new RelayCommand(ExecuteExit);
            VoteKickCommand = new RelayCommand(ExecuteVoteKickWindow);

            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            countdownTimer.Tick += CountdownTimer_Tick;

            InitializePlayerGameplay();

            LoadResources(); //TODO quit this

            Mediator.Register(Utilities.RECIVE_MESSAGE_GAME, OnReceiveMessage);
            Mediator.Register(Utilities.UPDATE_TIME_GAME, SetCountdownTime);
            Mediator.Register(Utilities.LOAD_GAME_PLAYER_LIST, LoadPlayerList);

            IsTradeGridVisible = true;
            UpdateTradeWindow();
        }

        private void InitializePlayerGameplay()
        {
            playerGameplay = new PlayerGameplayDto
            {
                CurrentSession = profile.CurrentSessionID,
                isRegistered = profile.IsRegistered,
                Id = profile.Id.Value
            };
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

        public async void ExecuteRollDiceAsync()
        {
            int diceValue = DiceRollNumbers();

            Mediator.Notify(Utilities.SHOW_ROLL_DICE_ANIMATION, diceValue);

            await serviceManager.GameServiceClient.ThrowDiceAsync(playerGameplay, AccountUtilities.CastChatGameToGameServiceGame(game), diceValue);
        }

        public int DiceRollNumbers()
        {
            Random random = new Random();
            int dice1 = random.Next(1, 7); 
            int dice2 = random.Next(1, 7);
            int total = dice1 + dice2;

            return total;
        }

        public void ExecuteNextTurn() 
        {
            bool result;

            App.Current.Dispatcher.InvokeAsync(async () =>
            {
               result = await serviceManager.GameServiceClient.GiveNextTurn(playerGameplay, AccountUtilities.CastChatGameToGameServiceGame(game));

                if(!result)
                {
                    Utilities.ShowMessgeServerLost();
                }
            });
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

        internal void ExecuteExit()
        {

            serviceManager.ChatServiceClient.LeftChatClient(game, profile.Name);

            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                serviceManager.GameServiceClient.ExitGame(playerGameplay, AccountUtilities.CastChatGameToGameServiceGame(game));
                await Task.Delay(5000);
                Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
                AccountUtilities.RestartGame();
            });
        }

        internal void ExecuteVoteKickWindow()
        {
            var kickPlayerWindow = new VoteKickPlayerWindow(game);
            kickPlayerWindow.ShowDialog();
        }



    }
}
