using CatanClient.Commands;
using CatanClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class FriendsViewModel : ViewModelBase
    {
        public ICommand AddFriendCommand { get; }

        public FriendsViewModel()
        {
            AddFriendCommand = new RelayCommand(ExecuteAddFriend);
        }

        private void ExecuteAddFriend(object parameter)
        {
            var addFriendWindow = new AddFriendWindow();

            addFriendWindow.ShowDialog();
        }

    }
}
