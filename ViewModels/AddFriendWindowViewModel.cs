using CatanClient.Commands;
using CatanClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class AddFriendWindowViewModel : ViewModelBase
    {
        public ICommand AddFriendCommand { get; }
        private string playerName;
        private readonly ServiceManager serviceManager;

        public string PlayerName
        {
            get => playerName;
            set
            {
                if (playerName != value)
                {
                    playerName = value;
                    OnPropertyChanged(nameof(PlayerName));
                }
            }
        }

        public AddFriendWindowViewModel(ServiceManager serviceManager)
        {
            AddFriendCommand = new RelayCommand(ExecuteAddFriend);
            this.serviceManager = serviceManager;
        }

        internal void ExecuteAddFriend(object parameter)
        {
            
        }

    }
}
