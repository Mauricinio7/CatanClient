using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.Controls
{
    internal class FriendRequestPlayerCardViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public bool IsOnline { get; set; }
        public ICommand AcceptCommand { get; }
        public ICommand RejectCommand { get; }

        private ServiceManager serviceManager;

        public FriendRequestPlayerCardViewModel(string playerName, bool isOnline, ServiceManager serviceManager)
        {
            PlayerName = playerName;
            IsOnline = isOnline;

            AcceptCommand = new RelayCommand(ExecuteAccept);
            RejectCommand = new RelayCommand(ExecuteReject);
            this.serviceManager = serviceManager;
        }

        private void ExecuteAccept(object parameter)
        {
            MessageBox.Show("Aceptar " + PlayerName);
        }

        private void ExecuteReject(object parameter)
        {
            MessageBox.Show("Aceptar " + PlayerName);
        }
    }
}
