using Autofac;
using CatanClient.Commands;
using CatanClient.Controls;
using CatanClient.ProfileService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class InviteFriendViewModel : ViewModelBase
    {
        public ObservableCollection<InvitePlayerCardViewModel> Friends { get; set; } = new ObservableCollection<InvitePlayerCardViewModel>();

        public List<ProfileDto> FriendsList { get; set; } = new List<ProfileDto>();
        public ICollectionView FriendsView { get; set; }
        private readonly ServiceManager serviceManager;
        private ProfileDto profile;
        private string accesKey;
        public ICommand CloseCommand { get; }   

        public InviteFriendViewModel(string accesKey, ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            this.accesKey = accesKey;

            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profile = AccountUtilities.CastAccountProfileToProfileService(profileDto);
            CloseCommand = new RelayCommand(ExecuteClose);


            if (GetAllFriend())
            {
                LoadFriendRequestList();
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }
        internal void ExecuteClose()
        {
            Mediator.Notify(Utilities.HIDE_INVITE_FRIENDS, null);
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

        public void LoadFriendRequestList()
        {
            foreach (var profileDto in FriendsList)
            {
                Friends.Add(App.Container.Resolve<InvitePlayerCardViewModel>(
                    new NamedParameter(Utilities.PROFILE, profileDto),
                    new NamedParameter(Utilities.ACCES_KEY, accesKey)));
            }

            FriendsView = CollectionViewSource.GetDefaultView(Friends);
            FriendsView.SortDescriptions.Add(new SortDescription(nameof(InvitePlayerCardViewModel.IsOnline), ListSortDirection.Descending));
        }
    }
}
