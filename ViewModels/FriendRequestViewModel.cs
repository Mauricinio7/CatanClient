﻿using Autofac;
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

namespace CatanClient.ViewModels
{
    internal class FriendRequestViewModel : ViewModelBase
    {
        public ObservableCollection<FriendRequestPlayerCardViewModel> Friends { get; set; }

        public List<ProfileDto> FriendsRequestsList { get; set; }
        public ICollectionView FriendsView { get; set; }
        private readonly ServiceManager serviceManager;
        private ProfileDto profile;

        public FriendRequestViewModel(ServiceManager serviceManager) 
        {
            this.serviceManager = serviceManager;

            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profile = AccountUtilities.CastAccountProfileToProfileService(profileDto);

            if(GetAllFriendRequests())
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
            var profileList = serviceManager.ProfileServiceClient.GetFriendRequestList(profile).ProfileDtos.ToList();

            Friends = new ObservableCollection<FriendRequestPlayerCardViewModel>();

            foreach (var profileDto in profileList)
            {
                bool isOnline = true;

                Friends.Add(App.Container.Resolve<FriendRequestPlayerCardViewModel>(
                    new NamedParameter("profile", profileDto),
                    new NamedParameter("isOnline", isOnline)));
            }

            FriendsView = CollectionViewSource.GetDefaultView(Friends);
            FriendsView.SortDescriptions.Add(new SortDescription(nameof(FriendRequestPlayerCardViewModel.IsOnline), ListSortDirection.Descending));
        }

    }
}
