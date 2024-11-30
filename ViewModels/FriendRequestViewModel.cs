using Autofac;
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

namespace CatanClient.ViewModels
{
    internal class FriendRequestViewModel : ViewModelBase
    {
        public ObservableCollection<FriendRequestPlayerCardViewModel> Friends { get; set; }

        public List<ProfileDto> FriendsRequestsList { get; set; } = new List<ProfileDto>();
        public ICollectionView FriendsView { get; set; }
        private readonly ServiceManager serviceManager;
        private ProfileDto profile;

        public FriendRequestViewModel(ServiceManager serviceManager) 
        {
            this.serviceManager = serviceManager;
            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profile = AccountUtilities.CastAccountProfileToProfileService(profileDto);
            InicializateFriendRequestsList();
        }

        private void InicializateFriendRequestsList()
        {
            if (GetAllFriendRequests())
            {
                LoadFriendRequestList();
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }
        }

        public bool GetAllFriendRequests()
        {
            OperationResultProfileListDto result;
            result = serviceManager.ProfileServiceClient.GetFriendRequestList(profile);

            if (result.IsSuccess)
            {
                FriendsRequestsList = result.ProfileDtos.ToList();
            }

            return result.IsSuccess;
        }

        public void LoadFriendRequestList()
        {
            List<ProfileDto> profileList = serviceManager.ProfileServiceClient.GetFriendRequestList(profile).ProfileDtos.ToList();

            Friends = new ObservableCollection<FriendRequestPlayerCardViewModel>();

            foreach (ProfileDto profileDto in profileList)
            {
                bool isOnline = profileDto.IsOnline;

                Friends.Add(App.Container.Resolve<FriendRequestPlayerCardViewModel>(
                    new NamedParameter(Utilities.PROFILE, profileDto)));
            }

            FriendsView = CollectionViewSource.GetDefaultView(Friends);
            FriendsView.SortDescriptions.Add(new SortDescription(nameof(FriendRequestPlayerCardViewModel.IsOnline), ListSortDirection.Descending));
        }

    }
}
