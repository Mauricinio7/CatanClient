using Autofac;
using CatanClient.Commands;
using CatanClient.Controls;
using CatanClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class FriendsViewModel : ViewModelBase
    {
        public ICommand AddFriendCommand { get; }
        public ObservableCollection<FriendPlayerCardViewModel> Friends { get; }
        public ICollectionView FriendsView { get; }

        public FriendsViewModel()
        {
            AddFriendCommand = new RelayCommand(ExecuteAddFriend);

            Friends = new ObservableCollection<FriendPlayerCardViewModel>
            {
                App.Container.Resolve<FriendPlayerCardViewModel>(
                    new NamedParameter("playerName", "TonyGamer54"), new NamedParameter("isOnline", true)),
                App.Container.Resolve<FriendPlayerCardViewModel>(
                    new NamedParameter("playerName", "GaboGamer81"), new NamedParameter("isOnline", false)),
                App.Container.Resolve<FriendPlayerCardViewModel>(
                    new NamedParameter("playerName", "YaelGamer91"), new NamedParameter("isOnline", true)),
                App.Container.Resolve<FriendPlayerCardViewModel>(
                    new NamedParameter("playerName", "NoelGamer761"), new NamedParameter("isOnline", false)),
                App.Container.Resolve<FriendPlayerCardViewModel>(
                    new NamedParameter("playerName", "BrayanGamer65"), new NamedParameter("isOnline", true))
            };


            FriendsView = CollectionViewSource.GetDefaultView(Friends);
            FriendsView.SortDescriptions.Add(new SortDescription(nameof(FriendPlayerCardViewModel.IsOnline), ListSortDirection.Descending));
        
    }

        private void ExecuteAddFriend(object parameter)
        {
            var addFriendWindow = new AddFriendWindow();

            addFriendWindow.ShowDialog();
        }


    }
}
