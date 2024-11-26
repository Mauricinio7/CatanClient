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
        private readonly ChatService.GameDto game;
        private readonly AccountService.ProfileDto profile;
        private int remainingTimeInSeconds;
        private bool turn;
        private bool isTradeGridVisible;
        private bool isBuildingSettlement;
        private string settlementButtonText;
        private PlayerGameplayDto playerGameplay;
        private bool hasRolledDice;
        public event Action<string> VertexOccupied;

        public ObservableCollection<string> HexTileImages { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<int> DiceNumbers { get; set; } = new ObservableCollection<int>();
        private readonly DispatcherTimer countdownTimer;
        public ICommand ShowTradeWindowCommand { get; }
        public ICommand RollDiceCommand { get; }
        public ICommand NextTurnCommand { get; }
        public ICommand HideTradeControlCommand { get; }
        public ICommand ShowTradeControlCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand VoteKickCommand { get; }
        public ICollectionView PlayersView { get; set; }
        public ICommand SelectVertexCommand { get; }
        public ICommand ToggleSettlementBuildingModeCommand { get; }

        public int SettlementsPlaced { get; set; } = 0; //TODO remove this


        private readonly ServiceManager serviceManager;
        List<HexTileDto> GameHexes = new List<HexTileDto>();
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


        

        public bool Turn
        {
            get => turn;
            set
            {
                if (turn != value)
                {
                    turn = value;
                    OnPropertyChanged(nameof(Turn));

                    if (turn)
                    {
                        HasRolledDice = false;
                    }
                    UpdateCommandStates();
                }
            }
        }

        public bool HasRolledDice
        {
            get => hasRolledDice;
            set
            {
                if (hasRolledDice != value)
                {
                    hasRolledDice = value;
                    OnPropertyChanged(nameof(HasRolledDice));
                    UpdateCommandStates();
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

        public bool IsBuildingSettlement
        {
            get => isBuildingSettlement;
            set
            {
                isBuildingSettlement = value;
                OnPropertyChanged(nameof(IsBuildingSettlement));
                ((RelayCommand)SelectVertexCommand).RaiseCanExecuteChanged();
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

        public string SettlementButtonText
        {
            get => settlementButtonText;
            set
            {
                settlementButtonText = value;
                OnPropertyChanged(nameof(SettlementButtonText));
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
            RollDiceCommand = new RelayCommand(_ => ExecuteRollDiceAsync(), _ => CanRollDice());
            NextTurnCommand = new RelayCommand(_ => ExecuteNextTurn(), _ => CanPlayTurn());
            HideTradeControlCommand = new RelayCommand(ExecuteHideTradeControl);
            ShowTradeControlCommand = new RelayCommand(ExecuteShowTradeControl);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);
            ExitCommand = new RelayCommand(ExecuteExit);
            VoteKickCommand = new RelayCommand(ExecuteVoteKickWindow);
            SelectVertexCommand = new RelayCommand(parameter => ExecuteSelectVertex(parameter), parameter => CanExecuteSelectVertex(parameter));
            ToggleSettlementBuildingModeCommand = new RelayCommand(_ => ExecuteToggleSettlementMode(), _ => CanPlayTurn());

            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            countdownTimer.Tick += CountdownTimer_Tick;

            InitializePlayerGameplay();

            LoadResources(); //TODO quit this

            Mediator.Register(Utilities.RECIVE_MESSAGE_GAME, OnReceiveMessage);
            Mediator.Register(Utilities.UPDATE_TIME_GAME, SetCountdownTime);
            Mediator.Register(Utilities.LOAD_GAME_PLAYER_LIST, LoadPlayerList);
            Mediator.Register(Utilities.LOAD_GAME_PLAYER_BOARD, hexes => LoadGameBoard(hexes));

            IsBuildingSettlement = false;
            SettlementButtonText = Utilities.GetTownText(CultureInfo.CurrentCulture.Name);

            IsTradeGridVisible = true;
            UpdateTradeWindow();
        }

        public void LoadGameBoard(object parameter)
        {
            if (!(parameter is List<HexTileDto> hexes))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            GameHexes = hexes;
            HexTileImages.Clear();

            foreach (var hex in hexes)
            {

                string imagePath = GetImagePathByResource(hex.Resource);
                if (!string.IsNullOrEmpty(imagePath))
                {
                    HexTileImages.Add(imagePath);
                    DiceNumbers.Add(hex.DiceValue);
                }
            }

            ((RelayCommand)SelectVertexCommand).RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(HexTileImages));
        }

        private void ExecuteToggleSettlementMode()
        {
            if (IsBuildingSettlement)
            {
                IsBuildingSettlement = false;
                SettlementButtonText = Utilities.GetTownText(CultureInfo.CurrentCulture.Name);
            }
            else
            {
                IsBuildingSettlement = true;
                SettlementButtonText = Utilities.GetCancelText(CultureInfo.CurrentCulture.Name);
            }
        }

        private static string GetImagePathByResource(string resourceName)
        {
            switch (resourceName)
            {
                case "Lunar Stone":
                    return "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/LunarStoneBiomec.png";
                case "Tritonium":
                    return "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/WoodBiomec.png";
                case "Alpha Nanofibers":
                    return "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/FiberBiomec.png";
                case "Epsilon Biomass":
                    return "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/BiomasaBiomec.png";
                case "GRX-810":
                    return "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/GRX-81Biomec.png";
                default:
                    return null;
            }
        }

        private void ExecuteSelectVertex(object parameter)
        {
            if (parameter is string tag && TryParseTag(tag, out int hexId, out int vertexId))
            {
                HexTileDto hex = GameHexes[hexId - 1];
                VertexDto vertex = hex.Vertices[vertexId - 1];

                vertex.IsOccupied = true;
                vertex.OwnerPlayerId = playerGameplay.Id;
                MessageBox.Show($"Asentamiento colocado en Hexágono {hexId}, Vértice {vertexId}.");

                ((RelayCommand)SelectVertexCommand).RaiseCanExecuteChanged();
                ExecuteToggleSettlementMode();
            }
        }

        private bool CanExecuteSelectVertex(object parameter)
        {
            if (!IsBuildingSettlement) 
                return false;

            if (GameHexes.Count == 0)
                return false;

            GameBoardStateDto gameBoardState = new GameBoardStateDto();
                gameBoardState.HexTiles = GameHexes.ToArray();
            gameBoardState.GameId = game.Id.Value;

            if (parameter is string tag && TryParseTag(tag, out int hexId, out int vertexId))
            {
                var hex = GameHexes[hexId - 1];
                if (hex == null)
                    return false;

                var vertex = hex.Vertices[vertexId - 1];
                if (vertex == null)
                    return false;

                if (vertex.IsOccupied)
                {
                    VertexOccupied?.Invoke(tag);
                    return false;
                }
                else
                {
                    return true;
                }
                //else if(GameRules.IsVertexAvailableForSettlement(gameBoardState, vertex.Id, playerGameplay.Id) || SettlementsPlaced < 2)
                //{
                    //return true;
                //}
            }

            return false;
        }

        private static bool TryParseTag(string tag, out int hexId, out int vertexId)
        {
            hexId = 0;
            vertexId = 0;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                var parts = tag.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[0], out hexId) && int.TryParse(parts[1], out vertexId))
                {
                    return true;
                }
            }
            return false;
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

        private bool CanRollDice()
        {
            return Turn && !hasRolledDice;
        }

        private bool CanPlayTurn()
        {
            return Turn && hasRolledDice;
        }

        private void UpdateCommandStates()
        {
            ((RelayCommand)RollDiceCommand).RaiseCanExecuteChanged();
            ((RelayCommand)NextTurnCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ShowTradeWindowCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ToggleSettlementBuildingModeCommand).RaiseCanExecuteChanged();
        }

        public static void ExecuteShowTradeWindow() 
        {
            TradeWindow tradeWindow = new TradeWindow();
            tradeWindow.ShowDialog();
        }

        public void ExecuteRollDiceAsync()
        {
            int diceValue = DiceRollNumbers();
            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                Mediator.Notify(Utilities.SHOW_ROLL_DICE_ANIMATION, diceValue);
                await serviceManager.GameServiceClient.ThrowDiceAsync(playerGameplay, AccountUtilities.CastChatGameToGameServiceGame(game), diceValue);
                HasRolledDice = true;
                UpdateCommandStates();
            });
        }

        public static int DiceRollNumbers()
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
                    if (player.ProfileTurnDto != null)
                    {
                        if(profile.Id == player.ProfileTurnDto.Id)
                        {
                            Turn = player.IsTurn;
                        }
                        OnlinePlayersList.Add(App.Container.Resolve<PlayerInGameCardViewModel>(
                            new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(player.ProfileTurnDto)),
                            new NamedParameter(Utilities.POINTS, player.Points),
                            new NamedParameter(Utilities.TURN, player.IsTurn)
                            ));
                    }
                    else
                    {
                        ProfileService.ProfileDto profileDto = AccountUtilities.CastGameProfileToProfileService(AccountUtilities.CastGuestAccountToGameServiceProfile(player.GuestAccountTurnDto));
                        if (profile.Id == player.GuestAccountTurnDto.Id)
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
