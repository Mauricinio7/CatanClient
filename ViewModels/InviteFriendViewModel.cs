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
using System.Windows.Data;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class InviteFriendViewModel : ViewModelBase
    {
        public ObservableCollection<FriendPlayerCardViewModel> Friends { get; set; } = new ObservableCollection<FriendPlayerCardViewModel>();

        public List<ProfileDto> FriendsList { get; set; } = new List<ProfileDto>();
        public ICollectionView FriendsView { get; set; }
        private readonly ServiceManager serviceManager;
        private ProfileDto profile;

        public InviteFriendViewModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;

            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profile = AccountUtilities.CastAccountProfileToProfileService(profileDto);


            if (GetAllFriend())
            {
                LoadFriendRequestList();
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
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
                bool isOnline = true;

                Friends.Add(App.Container.Resolve<FriendPlayerCardViewModel>(
                    new NamedParameter("playerName", profileDto.Name),
                    new NamedParameter("isOnline", isOnline)));
            }

            FriendsView = CollectionViewSource.GetDefaultView(Friends);
            FriendsView.SortDescriptions.Add(new SortDescription(nameof(FriendPlayerCardViewModel.IsOnline), ListSortDirection.Descending));
        }
    }
}
