using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CatanClient.Controls
{
    internal class InvitePlayerCardViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public bool IsOnline { get; set; }
        public ICommand InviteCommand { get; }

        private ServiceManager serviceManager;

        public InvitePlayerCardViewModel(string playerName, bool isOnline, ServiceManager serviceManager)
        {
            PlayerName = playerName;
            IsOnline = isOnline;

            InviteCommand = new RelayCommand(ExecuteInvite);
            this.serviceManager = serviceManager;
        }

        private void ExecuteInvite(object parameter)
        {
            MessageBox.Show("Invite: " + PlayerName);
        }

    }
}
