using CatanClient.Commands;
using CatanClient.Gameplay.Helpers;
using CatanClient.Gameplay.Views;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class GameFrameViewModel : ViewModelBase
    {
        public ICommand ShowTradeWindowCommand { get; }
        public ICommand HideTradeControlCommand { get; }
        public ICommand ShowTradeControlCommand { get; }

        private readonly ServiceManager serviceManager;
        public ObservableCollection<Resource> ResourcesToRequest { get; set; }
        public ObservableCollection<Resource> ResourcesToOffer { get; set; }
        public ObservableCollection<Resource> FilteredResourcesToRequest =>
           new ObservableCollection<Resource>(ResourcesToRequest.Where(r => r.Quantity > 0));

        public ObservableCollection<Resource> FilteredResourcesToOffer =>
            new ObservableCollection<Resource>(ResourcesToOffer.Where(r => r.Quantity > 0));

        private bool isTradeGridVisible;

        public bool IsTradeGridVisible
        {
            get => isTradeGridVisible;
            set
            {
                isTradeGridVisible = value;
                OnPropertyChanged(nameof(IsTradeGridVisible));
            }
        }



        public GameFrameViewModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            ShowTradeWindowCommand = new RelayCommand(ExecuteShowTradeWindow);
            HideTradeControlCommand = new RelayCommand(ExecuteHideTradeControl);
            ShowTradeControlCommand = new RelayCommand(ExecuteShowTradeControl);

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

            IsTradeGridVisible = true;
            UpdateTradeWindow();
        }

        internal void UpdateTradeWindow()
        {
            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(1000);
                IsTradeGridVisible = false;
            });
        }

        

        public void ExecuteShowTradeWindow()
        {
            TradeWindow tradeWindow = new TradeWindow();
            tradeWindow.ShowDialog();
        }

        public void ExecuteHideTradeControl()
        {
            IsTradeGridVisible = false;
        }

        public void ExecuteShowTradeControl()
        {
            IsTradeGridVisible = true;
        }

    }
}
