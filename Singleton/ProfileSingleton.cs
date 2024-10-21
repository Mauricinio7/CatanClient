﻿using CatanClient.AccountService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.Singleton
{
    internal class ProfileSingleton
    {
        private static ProfileSingleton instance;
        public ProfileDto Profile { get; private set; }

        private ProfileSingleton() { }

        public static ProfileSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProfileSingleton();
                }
                return instance;
            }
        }

        public void SetProfile(ProfileDto profile)
        {
            Profile = profile;
        }
    }
}
