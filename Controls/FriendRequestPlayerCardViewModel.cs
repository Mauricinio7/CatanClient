using CatanClient.Commands;
using CatanClient.ProfileService;
using CatanClient.Services;
using CatanClient.UIHelpers;
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
        public ProfileDto ReciverProfile { get; set; }

        public FriendRequestPlayerCardViewModel(string playerName, bool isOnline, ServiceManager serviceManager)
        {
            PlayerName = playerName;
            IsOnline = isOnline;
            this.serviceManager = serviceManager;
            ReciverProfile = AccountUtilities.CastAccountProfileToProfileService(serviceManager.ProfileSingleton.Profile);

            AcceptCommand = new RelayCommand(ExecuteAccept);
            RejectCommand = new RelayCommand(ExecuteReject);     
        }

        private void ExecuteAccept(object parameter)
        {
            bool result = serviceManager.ProfileServiceClient.AcceptFriendRequest(PlayerName, ReciverProfile);

            if (result)
            {
                MessageBox.Show("Se ha agregado correctamente como amigo al jugador: " + PlayerName);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        private void ExecuteReject(object parameter)
        {
            bool result = serviceManager.ProfileServiceClient.RejectFriendRequest(PlayerName, ReciverProfile);

            if (result)
            {
                MessageBox.Show("Se ha rechazado correctamente como amigo al jugador: " + PlayerName);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }
    }
}
