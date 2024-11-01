using Autofac;
using CatanClient.Commands;
using CatanClient.Controls;
using CatanClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class FriendsViewModel : ViewModelBase
    {
        public ICommand AddFriendCommand { get; }
        public ObservableCollection<FriendPlayerCardViewModel> Friends { get; }

        public FriendsViewModel()
        {
            AddFriendCommand = new RelayCommand(ExecuteAddFriend);

            Friends = new ObservableCollection<FriendPlayerCardViewModel>
            {
                App.Container.Resolve<FriendPlayerCardViewModel>(new NamedParameter("playerName", "Mauricinio7")),
                App.Container.Resolve<FriendPlayerCardViewModel>(new NamedParameter("playerName", "Carlos22")),
                App.Container.Resolve<FriendPlayerCardViewModel>(new NamedParameter("playerName", "LunaGirl"))
            };
        }

        private void ExecuteAddFriend(object parameter)
        {
            var addFriendWindow = new AddFriendWindow();

            addFriendWindow.ShowDialog();
        }

        public class FriendModel
        {
            public string PlayerName { get; set; }
        }

    }
}
