using Autofac;
using CatanClient.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.ViewModels
{
    internal class FriendRequestViewModel : ViewModelBase
    {
        public ObservableCollection<FriendRequestPlayerCardViewModel> Friends { get; }
        public ICollectionView FriendsView { get; }

        public FriendRequestViewModel() 
        {

            Friends = new ObservableCollection<FriendRequestPlayerCardViewModel>
            {
                App.Container.Resolve<FriendRequestPlayerCardViewModel>(
                    new NamedParameter("playerName", "TonyGamer54"), new NamedParameter("isOnline", true)),
                App.Container.Resolve<FriendRequestPlayerCardViewModel>(
                    new NamedParameter("playerName", "GaboGamer81"), new NamedParameter("isOnline", false)),
                App.Container.Resolve<FriendRequestPlayerCardViewModel>(
                    new NamedParameter("playerName", "YaelGamer91"), new NamedParameter("isOnline", true)),
                App.Container.Resolve<FriendRequestPlayerCardViewModel>(
                    new NamedParameter("playerName", "NoelGamer761"), new NamedParameter("isOnline", false)),
                App.Container.Resolve<FriendRequestPlayerCardViewModel>(
                    new NamedParameter("playerName", "BrayanGamer65"), new NamedParameter("isOnline", true))
            };

        }

    }
}
