using CatanClient.Properties;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.UIHelpers
{
    internal class Utilities
    {
        public const string DEFAULT_LANGUAGE = "en";
        public const string ALPHABET_EN = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const int VERIFICATION_CODE_PARTS_LENGTH = 3;
        public const int VERIFICATION_CODE_LENGTH = 9;
        public const string REGEX_PROFILE_NAME_VALIDATION = "^[a-zA-Z0-9 ]+$";
        public const string REGEX_PROFILE_EMAIL_VALIDATION = @"^[a-zA-Z0-9._-]+(?<!\.)@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string REGEX_PHONE_NUMBER_VALIDATION = "^[0-9]+$";
        public const string REGEX_PASSWORD_ACCOUNT_VALIDATION = "^[a-zA-Z0-9_]+$";
        public const string REGEX_DOUBLE_POINTS = "\\.\\.+";
        public const string STRING_CHAR_SPACE = " ";
        public const char STRING_CHAR_ARROBA = '@';
        public const string STRING_CHAR_DASH = "-";
        public const string STRING_CHAR_POINT = ".";
        public const string LOGGER_FILE_DIRECTORY = "C:/Users/mauricio/source/CatanClient/logs/errorlog.txt";
        public const string LANGUAGE_ENGLISH_FORMAT_RESX = "en";
        public const string LANGUAGE_ESPANISH_FORMAT_RESX = "es";
        public const string IPACCOUNTSERVICE = "http://192.168.1.127:8181/AccountService";
        public const string IP_GAME_SERVICE = "http://192.168.1.127:8191/GameService";
        public const string IP_CHAT_SERVICE = "net.tcp://192.168.1.127:8202/ChatService";
        public const string FADEOUTANIMATION = "FadeOutAnimation";
        public const string SHOWMAINMENUBACKGROUND = "ShowMainMenuBackgroundView";
        public const string SHOWMAINMENU = "ShowMainMenuView";
        public const string SHOWVERIFYACCOUNT = "ShowVerifyAccountView";
        public const string OCULTVERIFYACCOUNT = "OcultVerifyAccountView";
        public const string SHOWGAMELOBBY = "ShowGameLobbyView";
        public const string ANIMATEDGRID = "animatedGrid";
        public const string FADEINANIMATION = "FadeInAnimation";
        public const string SLIDEOUTFROMTOPANIMATION = "SlideOutFromTopAnimation";
        public const string SLIDEOUTFROMRIGHTANIMATION = "SlideOutFromRightAnimation";
        public const string SYSTEM_NAME = "System";

        public static string MessageDataBaseUnableToLoad(string language, Exception ex)
        {
            Log.Information(ex.Message);
            return Resources.ResourceManager.GetString("dialog_database_load_exception", new CultureInfo(language));
        }

        public static string MessageDataBaseUnableToLoad(string language)
        {                                           
            return Resources.ResourceManager.GetString("dialog_database_load_exception", new CultureInfo(language));
        }
        public static string TittleDataBaseUnableToLoad(string language)
        {
            return Resources.ResourceManager.GetString("dialog_database_load_exception", new CultureInfo(language));
        }
        public static string MessageDataBaseSuccessFullySave(string language)
        {
            return Resources.ResourceManager.GetString("dialog_success_save_database", new CultureInfo(language));
        }
        public static string MessageUnableToFindInDataBase(string language)
        {
            return Resources.ResourceManager.GetString("dialog_database_load_exception", new CultureInfo(language));
        }
        public static string MessageInvalidUsername(string language)
        {
            return Resources.ResourceManager.GetString("dialog_invalid_username_characters", new CultureInfo(language));
        }
        public static string MessageInvalidCaracters(string language)
        {
            return Resources.ResourceManager.GetString("dialog_invalid_characters_message", new CultureInfo(language));
        }
        public static string TittleInvalidCaracters(string language)
        {
            return Resources.ResourceManager.GetString("dialog_invalid_characters", new CultureInfo(language));
        }
        public static string MessageEmptyField(string language)
        {
            return Resources.ResourceManager.GetString("dialog_empty_fields_message", new CultureInfo(language));
        }
        public static string TittleEmptyField(string language)
        {
            return Resources.ResourceManager.GetString("dialog_empty_fields", new CultureInfo(language));
        }
        public static string MessageIncorrectPasswordOrUsername(string language)
        {
            return Resources.ResourceManager.GetString("dialog_invalid_username_password_message", new CultureInfo(language));
        }
        public static string TittleIncorrectPasswordOrUsername(string language)
        {
            return Resources.ResourceManager.GetString("dialog_invalid_username_password", new CultureInfo(language));
        }
        public static string TittleServerLostConnection(string language)
        {
            return Resources.ResourceManager.GetString("dialog_connection_server_exception", new CultureInfo(language));
        }
        public static string MessageServerLostConnection(string language)
        {
            return Resources.ResourceManager.GetString("dialog_connection_server_exception_message", new CultureInfo(language));
        }
        public static string MessageTokenSubject(string language)
        {
            return Resources.ResourceManager.GetString("subject_verification_code", new CultureInfo(language));
        }
        public static string TittleSuccess(string language)
        {
            return Resources.ResourceManager.GetString("dialog_success", new CultureInfo(language));
        }
        public static string MessageSuccessVerifyUser(string language)
        {
            return Resources.ResourceManager.GetString("dialog_succes_verify_user_message", new CultureInfo(language));
        }
        public static string TittleFail(string language)
        {
            return Resources.ResourceManager.GetString("dialog_fail", new CultureInfo(language));
        }
        public static string MessageFailVerifyUser(string language)
        {
            return Resources.ResourceManager.GetString("dialog_fail_verify_user_message", new CultureInfo(language));
        }
        public static string MessageUnableToSaveData(string language)
        {
            return Resources.ResourceManager.GetString("dialog_database_save_exception", new CultureInfo(language));
        }
        public static string DialogWelcome(string language)
        {
            return Resources.ResourceManager.GetString("dialog_welcome", new CultureInfo(language));
        }
        public static string MessageUnverifiedUser(string language)
        {
            return Resources.ResourceManager.GetString("dialog_unverified_user_message", new CultureInfo(language));
        }
        public static string TittleUnverifiedUser(string language)
        {
            return Resources.ResourceManager.GetString("dialog_unverified_user", new CultureInfo(language));
        }
        public static string MessagePlayerJoin(string language)
        {
            return Resources.ResourceManager.GetString("label_player_join", new CultureInfo(language));
        }
        public static string MessagePlayerLeft(string language)
        {
            return Resources.ResourceManager.GetString("label_player_left", new CultureInfo(language));
        }

    }
}
