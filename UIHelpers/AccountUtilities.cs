using CatanClient.AccountService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.UIHelpers
{
    public static class AccountUtilities
    {
        public static bool IsValidAccountName(string name)
        {
            string nameRegex = Utilities.REGEX_PROFILE_NAME_VALIDATION;

            return Regex.IsMatch(name, nameRegex);
        }

        public static bool IsValidAccountEmail(string email) //Refactor to have 1 return
        {
            string emailRegex = Utilities.REGEX_PROFILE_EMAIL_VALIDATION;

            if (!Regex.IsMatch(email, emailRegex)) return false;

            string[] parts = email.Split(Utilities.STRING_CHAR_ARROBA);

            if (parts.Length != 2) return false;

            string localPart = parts[0];
            string domainPart = parts[1];

            if (Regex.IsMatch(localPart, Utilities.REGEX_DOUBLE_POINTS) || Regex.IsMatch(domainPart, Utilities.REGEX_DOUBLE_POINTS)) return false;

            if (localPart.Contains("..")) return false;

            if (domainPart.StartsWith(Utilities.STRING_CHAR_DASH) || domainPart.EndsWith(Utilities.STRING_CHAR_DASH) ||
                domainPart.StartsWith(Utilities.STRING_CHAR_POINT) || domainPart.EndsWith(Utilities.STRING_CHAR_POINT)) return false;

            return true;
        }


        public static bool IsValidAccountPhoneNumber(string phoneNumber)
        {
            string phoneNumberRegex = Utilities.REGEX_PHONE_NUMBER_VALIDATION;

            return Regex.IsMatch((string)phoneNumber, phoneNumberRegex);
        }

        public static bool IsValidAccountPassword(string password)
        {
            string passwordRegex = Utilities.REGEX_PASSWORD_ACCOUNT_VALIDATION;
            return Regex.IsMatch(password, passwordRegex);
        }

        public static AccountDto CreateAccount(string email, string phoneNumber, string password, string name)
        {
            AccountDto account = new AccountDto
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Password = password,
                PreferredLanguage = CultureInfo.CurrentCulture.Name
                
            };

            return account;
        }

        public static GameService.GameDto CastChatGameToGameServiceGame(ChatService.GameDto chatGame)
        {
            GameService.GameDto gameServiceGame = new GameService.GameDto();
            gameServiceGame.Name = chatGame.Name;
            gameServiceGame.Id = chatGame.Id;
            gameServiceGame.IdAdminGame = chatGame.IdAdminGame;
            gameServiceGame.AccessKey = chatGame.AccessKey;
            gameServiceGame.MaxNumberPlayers = chatGame.MaxNumberPlayers;

            return gameServiceGame;
        }

        public static void RestartGame()
        {
            MessageBox.Show(Utilities.MessageRestartGame(CultureInfo.CurrentCulture.Name), Utilities.TittleRestart(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);

            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Process.Start(exePath);
            Application.Current.Shutdown();
        }

        public static ProfileService.ProfileDto CastAccountProfileToProfileService(AccountService.ProfileDto accountProfile)
        {
            ProfileService.ProfileDto profile = new ProfileService.ProfileDto
            {
                Id = accountProfile.Id,
                Name = accountProfile.Name,
                CurrentSessionID = accountProfile.CurrentSessionID,
                PreferredLanguage = CultureInfo.CurrentCulture.Name
            };

            return profile;
        }

        public static GameService.ProfileDto CastAccountProfileToGameService(AccountService.ProfileDto accountProfile)
        {
            GameService.ProfileDto profile = new GameService.ProfileDto
            {
                Id = accountProfile.Id,
                Name = accountProfile.Name,
                CurrentSessionID = accountProfile.CurrentSessionID,
                PreferredLanguage = CultureInfo.CurrentCulture.Name
            };

            return profile;
        }

        public static ProfileService.ProfileDto CastGameProfileToProfileService(GameService.ProfileDto gameProfile)
        {
            ProfileService.ProfileDto profile = new ProfileService.ProfileDto
            {
                Id = gameProfile.Id,
                Name = gameProfile.Name,
                CurrentSessionID = gameProfile.CurrentSessionID,
                PreferredLanguage = CultureInfo.CurrentCulture.Name
            };

            return profile;
        }
    }
}
