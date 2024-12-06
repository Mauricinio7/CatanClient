using Autofac;
using CatanClient.Commands;
using CatanClient.Controls;
using CatanClient.ProfileService;
using CatanClient.Services;
using CatanClient.UIHelpers;
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
        public ObservableCollection<FriendPlayerCardViewModel> Friends { get; set; } = new ObservableCollection<FriendPlayerCardViewModel>();
        public List<ProfileDto> FriendsList { get; set; } = new List<ProfileDto>();
        public ICollectionView FriendsView { get; set; }
        public ICommand AddFriendCommand { get; }
        public ICommand CloseFriendsCommand { get; }
        private readonly ServiceManager serviceManager;
        private readonly ProfileDto profile;

        public FriendsViewModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profile = AccountUtilities.CastAccountProfileToProfileService(profileDto);
            AddFriendCommand = new RelayCommand(ExecuteAddFriend);
            CloseFriendsCommand = new RelayCommand(ExecuteCloseFriends);
            InicializateFriendsList();
        }

        private void InicializateFriendsList()
        {
            if (GetAllFriend())
            {
                LoadFriendsList();
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        private static void ExecuteAddFriend(object parameter)
        {
            AddFriendWindow addFriendWindow = new AddFriendWindow();

            addFriendWindow.ShowDialog();
        }

        private static void ExecuteCloseFriends()
        {
            Mediator.Notify(Utilities.HIDE_FRIENDS, null);
        }

        public bool GetAllFriend()
        {
            OperationResultProfileListDto result;
            result = serviceManager.ProfileServiceClient.GetFriendList(profile);

            if (result.IsSuccess)
            {
                FriendsList = result.ProfileDtos.ToList();
            }

            return result.IsSuccess;
        }

        public void LoadFriendsList()
        {
            foreach (ProfileDto profileDto in FriendsList)
            {
                Friends.Add(App.Container.Resolve<FriendPlayerCardViewModel>(
                    new NamedParameter(Utilities.PROFILE, profileDto)));
            }

            FriendsView = CollectionViewSource.GetDefaultView(Friends);
            FriendsView.SortDescriptions.Add(new SortDescription(nameof(FriendPlayerCardViewModel.IsOnline), ListSortDirection.Descending));
        }


    }
}
