using Autofac;
using Autofac.Core;
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
using System.Numerics;
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
        private bool isBuildingRoad;
        private string roadButtonText;
        private bool isBuildingCity;
        private string cityButtonText;
        private string points;
        private int quantityLunarStone;
        private int quantityTritonium;
        private int quantityNanofiber;
        private int quantityBiomass;
        private int quantityGRX;
        private PlayerGameplayDto playerGameplay;
        private PlayerResourcesDto playerResources;
        private bool hasRolledDice;
        public event Action<string, bool, bool> VertexOccupied;
        public event Action<string, bool> EdgeOccupied;
        private List<PlayerResourcesDto> tradeResources = new List<PlayerResourcesDto>();


        public ObservableCollection<string> HexTileImages { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<int> DiceNumbers { get; set; } = new ObservableCollection<int>();
        private readonly DispatcherTimer countdownTimer;
        public ICommand ShowTradeWindowCommand { get; }
        public ICommand RollDiceCommand { get; }
        public ICommand NextTurnCommand { get; }
        public ICommand HideTradeControlCommand { get; }
        public ICommand AcceptTradeCommand { get; }
        public ICommand ShowTradeControlCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand VoteKickCommand { get; }
        public ICollectionView PlayersView { get; set; }
        public ICommand SelectVertexCommand { get; }
        public ICommand SelectEdgeCommand { get; }
        public ICommand ToggleSettlementBuildingModeCommand { get; }
        public ICommand ToggleRoadBuildingModeCommand { get; }
        public ICommand ToggleCityBuildingModeCommand { get; }


        private readonly ServiceManager serviceManager;
        List<HexTileDto> GameHexes = new List<HexTileDto>();
        public ObservableCollection<ChatMessage> Messages { get; set; }
        public ObservableCollection<PlayerInGameCardViewModel> OnlinePlayersList { get; set; } = new ObservableCollection<PlayerInGameCardViewModel>();
        private ObservableCollection<Resource> resourcesToRequest = new ObservableCollection<Resource>();
        private ObservableCollection<Resource> resourcesToOffer = new ObservableCollection<Resource>();
        public ObservableCollection<Resource> FilteredResourcesToRequest =>
           new ObservableCollection<Resource>(ResourcesToRequest.Where(r => r.Quantity > 0));

        public ObservableCollection<Resource> FilteredResourcesToOffer =>
            new ObservableCollection<Resource>(ResourcesToOffer.Where(r => r.Quantity > 0));

        public string TimeText => remainingTimeInSeconds > 0
    ? Utilities.GetTimeRemainingText(CultureInfo.CurrentCulture.Name, TimeSpan.FromSeconds(remainingTimeInSeconds).ToString(Utilities.TIME_FORMAT))
    : Utilities.GetAssigningTurnText(CultureInfo.CurrentCulture.Name);


        public ObservableCollection<Resource> ResourcesToRequest
        {
            get => resourcesToRequest;
            set
            {
                resourcesToRequest = value;
                OnPropertyChanged(nameof(ResourcesToRequest));
                OnPropertyChanged(nameof(FilteredResourcesToRequest));
            }
        }

        public ObservableCollection<Resource> ResourcesToOffer
        {
            get => resourcesToOffer;
            set
            {
                resourcesToOffer = value;
                OnPropertyChanged(nameof(ResourcesToOffer));
                OnPropertyChanged(nameof(FilteredResourcesToOffer));
            }
        }

        public bool Turn
        {
            get => turn;
            set
            {
                if (turn != value)
                {
                    turn = value;
                    OnPropertyChanged(nameof(Turn));

                    if (!turn)
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
        public string SettlementButtonText
        {
            get => settlementButtonText;
            set
            {
                settlementButtonText = value;
                OnPropertyChanged(nameof(SettlementButtonText));
            }
        }
        public bool IsBuildingRoad
        {
            get => isBuildingRoad;
            set
            {
                isBuildingRoad = value;
                OnPropertyChanged(nameof(IsBuildingRoad));
                ((RelayCommand)SelectEdgeCommand).RaiseCanExecuteChanged();
            }
        }
        public string RoadButtonText
        {
            get => roadButtonText;
            set
            {
                roadButtonText = value;
                OnPropertyChanged(nameof(RoadButtonText));
            }
        }
        public bool IsBuildingCity
        {
            get => isBuildingCity;
            set
            {
                isBuildingCity = value;
                OnPropertyChanged(nameof(IsBuildingCity));
                ((RelayCommand)SelectVertexCommand).RaiseCanExecuteChanged();
            }
        }
        public string CityButtonText
        {
            get => cityButtonText;
            set
            {
                cityButtonText = value;
                OnPropertyChanged(nameof(CityButtonText));
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

        public string Points
        {
            get => points;
            set
            {
                points = value;
                OnPropertyChanged(nameof(Points));
            }
        }

        public int QuantityLunarStone
        {
            get => quantityLunarStone;
            set
            {
                quantityLunarStone = value;
                OnPropertyChanged(nameof(QuantityLunarStone));
            }
        }
        public int QuantityTritonium
        {
            get => quantityTritonium;
            set
            {
                quantityTritonium = value;
                OnPropertyChanged(nameof(QuantityTritonium));
            }
        }

        public int QuantityNanofiber
        {
            get => quantityNanofiber;
            set
            {
                quantityNanofiber = value;
                OnPropertyChanged(nameof(QuantityNanofiber));
            }
        }
        public int QuantityBiomass
        {
            get => quantityBiomass;
            set
            {
                quantityBiomass = value;
                OnPropertyChanged(nameof(QuantityBiomass));
            }
        }

        public int QuantityGRX
        {
            get => quantityGRX;
            set
            {
                quantityGRX = value;
                OnPropertyChanged(nameof(QuantityGRX));
            }
        }


        public GameFrameViewModel(ChatService.GameDto gameDto, ServiceManager serviceManager)
        {
            game = gameDto;
            this.serviceManager = serviceManager;

            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage
                {
                    Content = Utilities.GetGameStartedText(CultureInfo.CurrentCulture.Name),
                    Name = Utilities.SYSTEM_NAME,
                    IsUserMessage = false
                }
            };

            profile = serviceManager.ProfileSingleton.Profile;


            ShowTradeWindowCommand = new RelayCommand(_ => ExecuteShowTradeWindow(), _ => CanPlayTurn());
            RollDiceCommand = new RelayCommand(_ => ExecuteRollDiceAsync(), _ => CanRollDice());
            NextTurnCommand = new RelayCommand(_ => ExecuteNextTurn(), _ => CanPlayTurn());
            HideTradeControlCommand = new RelayCommand(ExecuteHideTradeControl);
            AcceptTradeCommand = new RelayCommand(ExecuteAcceptTrade);
            ShowTradeControlCommand = new RelayCommand(ExecuteShowTradeControl);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage);
            ExitCommand = new RelayCommand(ExecuteExit);
            VoteKickCommand = new RelayCommand(ExecuteVoteKickWindow);
            SelectVertexCommand = new RelayCommand(parameter => ExecuteSelectVertex(parameter), parameter => CanExecuteSelectVertex(parameter));
            SelectEdgeCommand = new RelayCommand(parameter => ExecuteSelectEdge(parameter), parameter => CanExecuteSelectEdge(parameter));
            ToggleSettlementBuildingModeCommand = new RelayCommand(_ => ExecuteToggleSettlementMode(), _ => CanExecuteToggleSettlementMode());
            ToggleRoadBuildingModeCommand = new RelayCommand(_ => ExecuteToggleRoadMode(), _ => CanExecuteToggleRoadMode());
            ToggleCityBuildingModeCommand = new RelayCommand(_ => ExecuteToggleCityMode(), _ => CanExecuteToggleCityMode());

            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            countdownTimer.Tick += CountdownTimer_Tick;

            InitializePlayerGameplay();

            Mediator.Register(Utilities.RECIVE_MESSAGE_GAME, OnReceiveMessage);
            Mediator.Register(Utilities.UPDATE_TIME_GAME, SetCountdownTime);
            Mediator.Register(Utilities.LOAD_GAME_PLAYER_LIST, LoadPlayerList);
            Mediator.Register(Utilities.LOAD_GAME_PLAYER_BOARD, hexes => LoadGameBoard(hexes));
            Mediator.Register(Utilities.UPDATE_GAME_PLAYER_BOARD, hexes => UpdateGameBoard(hexes));
            Mediator.Register(Utilities.UPDATE_PLAYER_RESOURCES, resources => UpdatePlayerResources(resources));
            Mediator.Register(Utilities.LOAD_GAME_TRADE, resources => LoadTradeResources(resources));
            Mediator.Register(Utilities.HIDE_TRADE_CONTROL, ExecuteHideTradeControl);

            IsBuildingSettlement = false;
            SettlementButtonText = Utilities.GetTownText(CultureInfo.CurrentCulture.Name);
            IsBuildingRoad = false;
            RoadButtonText = Utilities.GetRoadText(CultureInfo.CurrentCulture.Name);
            isBuildingCity = false;
            CityButtonText = Utilities.GetCityText(CultureInfo.CurrentCulture.Name); 

            IsTradeGridVisible = true;
            UpdateTradeWindow();
        }

        public void UpdatePlayerResources(object parameter)
        {
            if (!(parameter is PlayerResourcesDto resources))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.playerResources = resources;
            QuantityLunarStone = resources.LunarStone.Quantity;
            QuantityTritonium = resources.Tritonium.Quantity;
            QuantityNanofiber = resources.AlphaNanofibers.Quantity;
            QuantityBiomass = resources.EpsilonBiomass.Quantity;
            QuantityGRX = resources.Grx810.Quantity;
        }

        public void LoadGameBoard(object parameter)
        {

            App.Current.Dispatcher.InvokeAsync(async () =>
            {

                if (!(parameter is List<HexTileDto> hexes))
                {
                    MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                GameHexes = hexes;
                HexTileImages.Clear();

                foreach (HexTileDto hex in hexes)
                {
                    await Task.Delay(100);
                    string imagePath = GetImagePathByResource(hex.Resource);
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        HexTileImages.Add(imagePath);
                        DiceNumbers.Add(hex.DiceValue);
                    }
                }

            ((RelayCommand)SelectEdgeCommand).RaiseCanExecuteChanged();
                ((RelayCommand)SelectVertexCommand).RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(HexTileImages));
            });

        }

        public void UpdateGameBoard(object parameter)
        {
            if (!(parameter is List<HexTileDto> hexes))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            GameHexes = hexes;

            ((RelayCommand)SelectEdgeCommand).RaiseCanExecuteChanged();
            ((RelayCommand)SelectVertexCommand).RaiseCanExecuteChanged();
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

        private bool CanExecuteToggleSettlementMode()
        {
            return !IsBuildingRoad && !IsBuildingCity && CanPlayTurn() && HaveResourcesToBuildSettlement();
        }

        private void ExecuteToggleRoadMode()
        {
            if (IsBuildingRoad)
            {
                IsBuildingRoad = false;
                RoadButtonText = Utilities.GetRoadText(CultureInfo.CurrentCulture.Name);
            }
            else
            {
                IsBuildingRoad = true;
                RoadButtonText = Utilities.GetCancelText(CultureInfo.CurrentCulture.Name);
            }
        }

        private bool CanExecuteToggleRoadMode()
        {
            return !IsBuildingSettlement && !IsBuildingCity && CanPlayTurn() && HaveResourcesToBuildRoad();
        }
        private void ExecuteToggleCityMode()
        {
            if (IsBuildingCity)
            {
                IsBuildingCity = false;
                CityButtonText = Utilities.GetCityText(CultureInfo.CurrentCulture.Name);
            }
            else
            {
                IsBuildingCity = true;
                CityButtonText = Utilities.GetCancelText(CultureInfo.CurrentCulture.Name);
            }
        }

        private bool CanExecuteToggleCityMode()
        {
            return !IsBuildingRoad && !IsBuildingSettlement && CanPlayTurn() && HaveResourcesToBuildCity();
        }

        internal bool HaveResourcesToBuildSettlement()
        {
            return playerResources.Tritonium.Quantity >= 1 && playerResources.AlphaNanofibers.Quantity >= 1 && playerResources.EpsilonBiomass.Quantity >= 1 && playerResources.Grx810.Quantity >= 1;
        }
        internal bool HaveResourcesToBuildRoad()
        {
            return playerResources.Tritonium.Quantity >= 1 && playerResources.Grx810.Quantity >= 1;
        }
        internal bool HaveResourcesToBuildCity()
        {
            return playerResources.EpsilonBiomass.Quantity >= 2 && playerResources.LunarStone.Quantity >= 3;
        }

        private static string GetImagePathByResource(string resourceName)
        {
            string path;

            switch (resourceName)
            {
                case Utilities.LUNAR_STONE:
                    path = Utilities.LUNAR_STONE_BIOME_IMAGE_PATH;
                    break;
                case Utilities.TRITONIUM:
                    path = Utilities.TRITONIUM_BIOME_IMAGE_PATH;
                    break;
                case Utilities.ALPHA_NANOFIBERS:
                    path = Utilities.ALPHA_NANOFIBERS_BIOME_IMAGE_PATH;
                    break;
                case Utilities.EPSILON_BIOMASS:
                    path = Utilities.EPSILON_BIOMASS_BIOME_IMAGE_PATH;
                    break;
                case Utilities.GRX_810:
                    path = Utilities.GRX_810_BIOME_IMAGE_PATH;
                    break;
                default:
                    path = string.Empty;
                    break;
            }

            return path;
        }

        private void ExecuteSelectVertex(object parameter)
        {
            if (parameter is string tag && TryParseTag(tag, out int hexIndex, out int vertexIndex))
            {
                HexTileDto hex = GameHexes[hexIndex - 1];
                VertexDto vertex = hex.Vertices[vertexIndex - 1];
                int hexId = hex.Id;
                int vertexId = vertex.Id;

                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                App.Current.Dispatcher.InvokeAsync(async () =>
                {
                    if (IsBuildingCity)
                    {
                        await BuilCityAsync(hexId, vertexId);
                    }
                    else if (IsBuildingSettlement)
                    {
                        await BuilSettlementAsync(hexId, vertexId);
                    }

                ((RelayCommand)SelectVertexCommand).RaiseCanExecuteChanged();
                });
            }
        }

        internal async Task BuilCityAsync(int hexId, int vertexId)
        {
            OperationResultDto result;
            PiecePlacementDto placement = new PiecePlacementDto();
            placement.PieceType = Utilities.CITY;
            placement.TargetHexId = hexId;
            placement.TargetVertexId = vertexId;

            MessageBox.Show("Se construye en, ID hex : " + hexId + "ID Vertex: " + vertexId);

            try
            {
                result = await serviceManager.GameServiceClient.PlacePiceAsync(placement, playerGameplay, AccountUtilities.CastChatGameToGameServiceGame(game));
                if (result.IsSuccess)
                {
                    Utilities.ShowMessageSuccessfulBuild();
                    ExecuteToggleCityMode();
                }
                else
                {
                    Utilities.ShowMessageUnsuccessfulBuild();
                }
            }
            catch (TimeoutException)
            {
                Utilities.ShowMessgeServerLost();
            }
        }
        internal async Task BuilSettlementAsync(int hexId, int vertexId)
        {
            OperationResultDto result;
            PiecePlacementDto placement = new PiecePlacementDto();
            placement.PieceType = Utilities.SETTLEMENT;
            placement.TargetHexId = hexId;
            placement.TargetVertexId = vertexId;

            MessageBox.Show("Se construye en, ID hex : " + hexId + "ID Vertex: " + vertexId);

            try
            {
                result = await serviceManager.GameServiceClient.PlacePiceAsync(placement, playerGameplay, AccountUtilities.CastChatGameToGameServiceGame(game));

                if (result.IsSuccess)
                {
                    Utilities.ShowMessageSuccessfulBuild();
                    ExecuteToggleSettlementMode();
                }
                else
                {
                    Utilities.ShowMessageUnsuccessfulBuild();
                }
            }
            catch (TimeoutException)
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        private bool CanExecuteSelectVertex(object parameter)
        {
            if (GameHexes.Count == 0 || !(parameter is string tag) || !TryParseTag(tag, out int hexIndex, out int vertexIndex))
                return false;

            HexTileDto hex = GetHexAtIndex(hexIndex - 1);
            VertexDto vertex = GetVertexAtIndex(hex, vertexIndex - 1);

            if (vertex == null)
                return false;

            if (vertex.IsOccupied)
                HandleOccupiedVertex(tag, vertex);

            return IsBuildingCity
                ? CanBuildCity(vertex)
                : IsBuildingSettlement && CanBuildSettlement(vertex);
        }

        private HexTileDto GetHexAtIndex(int index)
        {
            return index >= 0 && index < GameHexes.Count ? GameHexes[index] : null;
        }

        private static VertexDto GetVertexAtIndex(HexTileDto hex, int index)
        {
            return hex != null && index >= 0 && index < hex.Vertices.Length ? hex.Vertices[index] : null;
        }

        private void HandleOccupiedVertex(string tag, VertexDto vertex)
        {
            bool isOwner = vertex.OwnerPlayerId == playerGameplay.Id;
            VertexOccupied?.Invoke(tag, vertex.IsCity, isOwner);
        }

        private bool CanBuildCity(VertexDto vertex)
        {
            return vertex.IsOccupied && vertex.OwnerPlayerId == playerGameplay.Id && !vertex.IsCity;
        }

        private static bool CanBuildSettlement(VertexDto vertex)
        {
            return !vertex.IsOccupied;
        }




        private static bool TryParseTag(string tag, out int hexId, out int vertexId)
        {
            hexId = 0;
            vertexId = 0;
            bool flag = false;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                string[] parts = tag.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[0], out hexId) && int.TryParse(parts[1], out vertexId))
                {
                    flag = true;
                }
            }
            return flag;
        }

        private void ExecuteSelectEdge(object parameter)
        {
            if (parameter is string tag && TryParseTagEdge(tag, out int hexIndex, out int edgeIndex))
            {
                HexTileDto hex = GameHexes[hexIndex - 1];
                EdgeDto edge = hex.Edges[edgeIndex - 1];
                int hexId = hex.Id;
                int edgeId = edge.Id;

                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                App.Current.Dispatcher.InvokeAsync(async () =>
                {
                    await BuilRoadAsync(hexId, edgeId);
                });
            }
        }

        internal async Task BuilRoadAsync(int hexId, int edgeId)
        {
            OperationResultDto result;
            PiecePlacementDto placement = new PiecePlacementDto();
            placement.PieceType = Utilities.ROAD;
            placement.TargetHexId = hexId;
            placement.TargetEdgeId = edgeId;

            MessageBox.Show("Se construye en, ID hex : " + hexId + "ID Edge: " + edgeId);

            try
            {
                result = await serviceManager.GameServiceClient.PlacePiceAsync(placement, playerGameplay, AccountUtilities.CastChatGameToGameServiceGame(game));

                if (result.IsSuccess)
                {
                    Utilities.ShowMessageSuccessfulBuild();

                    ExecuteToggleRoadMode();
                }
                else
                {
                    Utilities.ShowMessageUnsuccessfulBuild();
                }
            }
            catch (TimeoutException)
            {
                Utilities.ShowMessgeServerLost();
            }
                    ((RelayCommand)SelectEdgeCommand).RaiseCanExecuteChanged();
        }

        private bool CanExecuteSelectEdge(object parameter)
        {
            bool canExecute = false;

            if (IsBuildingRoad && GameHexes.Count > 0 && parameter is string tag && TryParseTagEdge(tag, out int hexIndex, out int edgeIndex))
            {
                HexTileDto hex = GameHexes[hexIndex - 1];
                if (hex != null)
                {
                    EdgeDto edge = hex.Edges[edgeIndex - 1];
                    if (edge != null)
                    {
                        if (edge.IsOccupied)
                        {
                            bool isOwner = edge.OwnerPlayerId == playerGameplay.Id;
                            EdgeOccupied?.Invoke(tag, isOwner);
                        }
                        else
                        {
                            canExecute = true;
                        }
                    }
                }
            }

            return canExecute;
        }

        private static bool TryParseTagEdge(string tag, out int hexIndex, out int edgeIndex)
        {
            hexIndex = 0;
            edgeIndex = 0;
            bool flag = false;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                string[] parts = tag.Split(',');
                if (parts.Length == 3 && parts[0] == "E" && int.TryParse(parts[1], out hexIndex) && int.TryParse(parts[2], out edgeIndex))
                {
                    flag = true;
                }
            }
            return flag;
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

        private void LoadTradeResources(object parameter) 
        {
            if (!(parameter is List<PlayerResourcesDto> resources))
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            tradeResources = resources;
            PlayerResourcesDto receiveResources = resources[1];
            PlayerResourcesDto sendResources = resources[0];

            ResourcesToRequest = new ObservableCollection<Resource>
            {
                new Resource { Name = Utilities.LUNAR_STONE, ImageSource = Utilities.LUNAR_STONE_IMAGE_PATH, Quantity = resources[1].LunarStone.Quantity},
                new Resource { Name = Utilities.TRITONIUM, ImageSource = Utilities.TRITONIUM_IMAGE_PATH, Quantity = resources[1].Tritonium.Quantity},
                new Resource { Name = Utilities.ALPHA_NANOFIBERS, ImageSource = Utilities.ALPHA_NANOFIBERS_IMAGE_PATH, Quantity = resources[1].AlphaNanofibers.Quantity },
                new Resource { Name = Utilities.EPSILON_BIOMASS, ImageSource =  Utilities.EPSILON_BIOMASS_IMAGE_PATH, Quantity = resources[1].EpsilonBiomass.Quantity },
                new Resource { Name = Utilities.GRX_810, ImageSource = Utilities.GRX_810_IMAGE_PATH, Quantity = resources[1].Grx810.Quantity}
            };

            ResourcesToOffer = new ObservableCollection<Resource>
            {
                new Resource { Name = Utilities.LUNAR_STONE, ImageSource = Utilities.LUNAR_STONE_IMAGE_PATH, Quantity = resources[0].LunarStone.Quantity},
                new Resource { Name = Utilities.TRITONIUM, ImageSource = Utilities.TRITONIUM_IMAGE_PATH, Quantity = resources[0].Tritonium.Quantity},
                new Resource { Name = Utilities.ALPHA_NANOFIBERS, ImageSource = Utilities.ALPHA_NANOFIBERS_IMAGE_PATH, Quantity =   resources[0].AlphaNanofibers.Quantity },
                new Resource { Name = Utilities.EPSILON_BIOMASS, ImageSource =  Utilities.EPSILON_BIOMASS_IMAGE_PATH, Quantity = resources[0].EpsilonBiomass.Quantity },
                new Resource { Name = Utilities.GRX_810, ImageSource = Utilities.GRX_810_IMAGE_PATH, Quantity = resources[0].Grx810.Quantity}
            };

            IsTradeGridVisible = true;
        }

        private void ExecuteAcceptTrade()
        {
            if(tradeResources.Count <= 0)
            {
                MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                App.Current.Dispatcher.InvokeAsync(async () =>
                {
                    Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                    PlayerResourcesDto sendResources = tradeResources[0];
                    PlayerResourcesDto receiveResources = tradeResources[1];
                    sendResources.PlayerId = profile.Id.Value;

                    OperationResultDto result;
                    result = await serviceManager.GameServiceClient.AcceptTradeRequestAsync(sendResources, receiveResources, AccountUtilities.CastChatGameToGameServiceGame(game));
                    if (!result.IsSuccess)
                    {
                        MessageBox.Show(Utilities.MessageTradingError(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                });
            }
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
            ((RelayCommand)ToggleRoadBuildingModeCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ToggleCityBuildingModeCommand).RaiseCanExecuteChanged();
        }

        public void ExecuteShowTradeWindow() 
        {
            TradeWindow tradeWindow = new TradeWindow(AccountUtilities.CastChatGameToGameServiceGame(game));
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

        public void ExecuteHideTradeControl(object parameter)
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
                        LoadPlayerProfile(player);
                    }
                    else
                    {
                        LoadPlayerGuest(player);
                    }
                }

                PlayersView = CollectionViewSource.GetDefaultView(OnlinePlayersList);
                PlayersView.SortDescriptions.Add(new SortDescription(nameof(PlayerInGameCardViewModel.Turn), ListSortDirection.Descending));
            }
            
        }

        internal void LoadPlayerProfile(PlayerTurnStatusDto player)
        {
            if (profile.Id == player.ProfileTurnDto.Id)
            {
                Turn = player.IsTurn;
                Points = player.Points.ToString();
            }
            OnlinePlayersList.Add(App.Container.Resolve<PlayerInGameCardViewModel>(
                new NamedParameter(Utilities.PROFILE, AccountUtilities.CastGameProfileToProfileService(player.ProfileTurnDto)),
                new NamedParameter(Utilities.POINTS, player.Points),
                new NamedParameter(Utilities.TURN, player.IsTurn)
                ));
        }

        internal void LoadPlayerGuest(PlayerTurnStatusDto player)
        {
            ProfileService.ProfileDto profileDto = AccountUtilities.CastGameProfileToProfileService(AccountUtilities.CastGuestAccountToGameServiceProfile(player.GuestAccountTurnDto));
            if (profile.Id == player.GuestAccountTurnDto.Id)
            {
                Turn = player.IsTurn;
                Points = player.Points.ToString();
            }
            OnlinePlayersList.Add(App.Container.Resolve<PlayerInGameCardViewModel>(
                new NamedParameter(Utilities.PROFILE, profileDto),
                new NamedParameter(Utilities.POINTS, player.Points),
                new NamedParameter(Utilities.TURN, player.IsTurn)
                ));
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
            if (string.IsNullOrWhiteSpace(NewMessage))
            {
                NewMessage = string.Empty;
            }
            else
            {
                serviceManager.ChatServiceClient.SendMessageToServer(game, profile.Name, NewMessage);
                NewMessage = string.Empty;
            }
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
            VoteKickPlayerWindow kickPlayerWindow = new VoteKickPlayerWindow(game);
            kickPlayerWindow.ShowDialog();
        }



    }
}
