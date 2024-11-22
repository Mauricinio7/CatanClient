using CatanClient.AccountService;
using CatanClient.GameService;
using CatanClient.Services;
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
    internal static class AccountUtilities
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

            return Regex.IsMatch(phoneNumber, phoneNumberRegex);
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

        public static GameService.ProfileDto CastGuestAccountToGameServiceProfile(GuestAccountDto guestAccount)
        {
            GameService.ProfileDto profile = new GameService.ProfileDto
            {
                Id = guestAccount.Id,
                Name = guestAccount.Name,
                PreferredLanguage = CultureInfo.CurrentCulture.Name,
                IsRegistered = false,
                IsOnline = true,
                isReadyToPlay = guestAccount.isReadyToPlay
            };

            return profile;
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
                PreferredLanguage = CultureInfo.CurrentCulture.Name,
                IsRegistered = accountProfile.IsRegistered,
                IsOnline = accountProfile.IsOnline,
                PictureVersion = accountProfile.PictureVersion
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
                PreferredLanguage = CultureInfo.CurrentCulture.Name,
                IsRegistered = accountProfile.IsRegistered,
                IsOnline = accountProfile.IsOnline,
                PictureVersion = accountProfile.PictureVersion
            };

            return profile;
        }
        public static PlayerGameplayDto CastAccountProfileToPlayerGameplay(AccountService.ProfileDto accountProfile)
        {
            PlayerGameplayDto playerGameplay = new PlayerGameplayDto
            {
                Id = (int)accountProfile.Id,
                isRegistered = accountProfile.IsRegistered,
                CurrentSession = accountProfile.CurrentSessionID
            };

            return playerGameplay;
        }

        public static ProfileService.ProfileDto CastGameProfileToProfileService(GameService.ProfileDto gameProfile)
        {
            ProfileService.ProfileDto profile = new ProfileService.ProfileDto
            {
                Id = gameProfile.Id,
                Name = gameProfile.Name,
                CurrentSessionID = gameProfile.CurrentSessionID,
                PreferredLanguage = CultureInfo.CurrentCulture.Name,
                IsRegistered = gameProfile.IsRegistered,
                IsOnline = gameProfile.IsOnline,
                PictureVersion = gameProfile.PictureVersion
            };

            return profile;
        }

        public static GameService.GuestAccountDto CastAccountProfileToGuestAccount(AccountService.ProfileDto accountProfile)
        {
            GameService.GuestAccountDto guestAccountDto = new GameService.GuestAccountDto
            {
                Id = accountProfile.Id,
                Name = accountProfile.Name,
                PreferredLanguage = CultureInfo.CurrentCulture.Name,
            };


            return guestAccountDto;
        }

        public static async Task<bool> ResendVerificationCodeAsync(ServiceManager serviceManager, AccountDto account)
        {
            bool result = await serviceManager.AccountServiceClient.ResendCodeAsync(account);

            if (result)
            {
                MessageBox.Show(Utilities.MessageSuccesSendVerificationCode(CultureInfo.CurrentCulture.Name),
                    Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Utilities.ShowMessgeServerLost();
            }

            return result;
        }

        public static void ValidateVerificationCode(string verificationCode)
        {
            if (string.IsNullOrWhiteSpace(verificationCode))
            {
                throw new InvalidOperationException(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name));
            }
        }

        public static void ValidatePassword(string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                throw new InvalidOperationException(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name));
            }

            if (!AccountUtilities.IsValidAccountPassword(password))
            {
                throw new InvalidOperationException(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name));
            }

            if (password != confirmPassword)
            {
                throw new InvalidOperationException(Utilities.MessagePasswordNotMacth(CultureInfo.CurrentCulture.Name));
            }
        }

        public static async Task<bool> VerifyAccountAsync(ServiceManager serviceManager, AccountDto account, string verificationCode)
        {
            bool status = await serviceManager.AccountServiceClient.VerifyUserAccountAsync(account, verificationCode);

            if (status)
            {
                MessageBox.Show(Utilities.MessageSuccessVerifyUser(CultureInfo.CurrentCulture.Name),
                    Utilities.TittleSuccess(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(Utilities.MessageFailVerifyUser(CultureInfo.CurrentCulture.Name),
                    Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return status;
        }

        public static bool VerificateChangePassword(string Password, string ConfirmPassword)
        {
            bool flag = false;

            if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                if (AccountUtilities.IsValidAccountPassword(Password))
                {
                    if (Password == ConfirmPassword)
                    {
                        flag = true;
                    }
                    else
                    {
                        MessageBox.Show(Utilities.MessagePasswordNotMacth(CultureInfo.CurrentCulture.Name), Utilities.TittleFail(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name), Utilities.TittleInvalidCaracters(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return flag;
        }
    }
}
