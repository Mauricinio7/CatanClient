using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.Controls
{
    internal class FriendPlayerCardViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public ICommand DeleteCommand { get; }

        private ServiceManager serviceManager;

        public FriendPlayerCardViewModel(string playerName, ServiceManager serviceManager)
        {
            PlayerName = playerName;

            DeleteCommand = new RelayCommand(ExecuteDelete);
            this.serviceManager = serviceManager;
        }

        private void ExecuteDelete(object parameter)
        {
            MessageBox.Show("Delete " + PlayerName);
        }
    }
}
