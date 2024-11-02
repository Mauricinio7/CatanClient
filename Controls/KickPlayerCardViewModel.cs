using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CatanClient.Controls
{
    internal class KickPlayerCardViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public ICommand KickCommand { get; }

        private ServiceManager serviceManager;

        public KickPlayerCardViewModel(string playerName, ServiceManager serviceManager)
        {
            PlayerName = playerName;

            KickCommand = new RelayCommand(ExecuteKick);
            this.serviceManager = serviceManager;
        }

        private void ExecuteKick(object parameter)
        {
            MessageBox.Show("Expulsar: " + PlayerName);
        }
    }
}
