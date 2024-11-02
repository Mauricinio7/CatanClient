using Autofac;
using CatanClient.Commands;
using CatanClient.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CatanClient.ViewModels
{
    internal class InviteFriendViewModel : ViewModelBase
    {
        public ObservableCollection<InvitePlayerCardViewModel> Friends { get; }
        public ICollectionView FriendsView { get; }

        public InviteFriendViewModel()
        {
            Friends = new ObservableCollection<InvitePlayerCardViewModel>
            {
                    App.Container.Resolve<InvitePlayerCardViewModel>(
                        new NamedParameter("playerName", "TonyGamer54"), new NamedParameter("isOnline", true)),
                App.Container.Resolve<InvitePlayerCardViewModel>(
                    new NamedParameter("playerName", "GaboGamer81"), new NamedParameter("isOnline", false)),
                App.Container.Resolve<InvitePlayerCardViewModel>(
                    new NamedParameter("playerName", "YaelGamer91"), new NamedParameter("isOnline", true)),
                App.Container.Resolve<InvitePlayerCardViewModel>(
                    new NamedParameter("playerName", "NoelGamer761"), new NamedParameter("isOnline", false)),
                App.Container.Resolve<InvitePlayerCardViewModel>(
                    new NamedParameter("playerName", "BrayanGamer65"), new NamedParameter("isOnline", true))
            };


            FriendsView = CollectionViewSource.GetDefaultView(Friends);
            FriendsView.SortDescriptions.Add(new SortDescription(nameof(InvitePlayerCardViewModel.IsOnline), ListSortDirection.Descending));

        }
    }
}
