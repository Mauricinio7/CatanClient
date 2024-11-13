using CatanClient.Commands;
using CatanClient.Gameplay.Helpers;
using CatanClient.Services;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.Gameplay.ViewModels
{
    internal class TradeWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Resource> ResourcesToOffer { get; set; }
        public ObservableCollection<Resource> ResourcesToRequest { get; set; }

        public ICommand SendTradeCommand { get; }
        private readonly ServiceManager serviceManager;

        public TradeWindowViewModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;

            ResourcesToOffer = new ObservableCollection<Resource>
            {
                new Resource { Name = "Lunar Stone", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/LunarStone.png" },
                new Resource { Name = "Tritonium", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/TritoniumWood.png" },
                new Resource { Name = "Alpha Nanofibers", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/AlphaNanofibers.png" },
                new Resource { Name = "Epsilon Biomass", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/EpsilonGrain.png" },
                new Resource { Name = "GRX-810", ImageSource = "pack://application:,,,/Gameplay/Resources/Images/GameResources/GRX-810Stone.png" }
            };

            ResourcesToRequest = new ObservableCollection<Resource>(ResourcesToOffer.Select(r => new Resource { Name = r.Name, ImageSource = r.ImageSource }));

            SendTradeCommand = new RelayCommand(OnSendTrade);
        }

        private void OnSendTrade(object parameter)
        {
            StringBuilder tradeDetails = new StringBuilder("Recursos a Solicitar:\n");
            foreach (var resource in ResourcesToRequest)
            {
                tradeDetails.AppendLine($"{resource.Name}: {resource.Quantity}");
            }

            tradeDetails.AppendLine("\nRecursos a Ofrecer:");
            foreach (var resource in ResourcesToOffer)
            {
                tradeDetails.AppendLine($"{resource.Name}: {resource.Quantity}");
            }

            MessageBox.Show(tradeDetails.ToString(), "Detalles de Intercambio", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
