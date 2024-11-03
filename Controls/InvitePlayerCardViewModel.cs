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
using System.Windows.Controls;
using System.Windows.Input;

namespace CatanClient.Controls
{
    internal class InvitePlayerCardViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public string AccesKey { get; set; }
        public bool IsOnline { get; set; }
        public ICommand InviteCommand { get; }

        private ServiceManager serviceManager;
        public ProfileDto SenderProfile { get; set; }

        public InvitePlayerCardViewModel(string playerName, bool isOnline, string accesKey,ServiceManager serviceManager)
        {
            PlayerName = playerName;
            IsOnline = isOnline;
            AccesKey = accesKey;

            InviteCommand = new RelayCommand(ExecuteInvite);
            this.serviceManager = serviceManager;
            SenderProfile = AccountUtilities.CastAccountProfileToProfileService(serviceManager.ProfileSingleton.Profile);
        }

        private void ExecuteInvite(object parameter)
        {
            bool result = serviceManager.ProfileServiceClient.InviteFriendToGame(PlayerName, SenderProfile, AccesKey);

            if(result)
            {
                MessageBox.Show("Se ha enviado la invitación correctamente al jugador: " + PlayerName);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

    }
}
