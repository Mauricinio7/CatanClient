using CatanClient.Commands;
using CatanClient.Gameplay.Views;
using CatanClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class GameFrameViewModel : ViewModelBase
    {
        public ICommand ShowTradeWindowCommand { get; }
        private readonly ServiceManager serviceManager;

        public GameFrameViewModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            ShowTradeWindowCommand = new RelayCommand(ExecuteShowTradeWindow);
        }

        public void ExecuteShowTradeWindow()
        {
            TradeWindow tradeWindow = new TradeWindow();
            tradeWindow.ShowDialog();
        }

    }
}
