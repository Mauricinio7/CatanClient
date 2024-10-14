using CatanClient.Properties;
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
        public const int SALT_SIZE = 16;
        public const string DEFAULT_LANGUAGE = "en";
        public const string ALPHABET_EN = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string SMTP_CLIENT_HOST = "smtp.gmail.com";
        public const string SMTP_CLIENT_ENVIROMENT_NAME = "EMAIL_USERNAME";
        public const string SMTP_CLIENT_ENVIROMENT_PASSWORD = "EMAIL_PASSWORD";
        public const string TWILIO_ENVIROMENT_ID = "TWILIO_ACCOUNT_SID";
        public const string TWILIO_ENVIROMENT_TOKEN = "TWILIO_AUTH_TOKEN";
        public const string TWILIO_PHONE_NUMBER = "TWILIO_PHONE_NUMBER";
        public const int SMTP_CLIENT_TIME_OUT = 20000;
        public const int SMTP_CLIENT_PORT = 587;
        public const int VERIFICATION_CODE_PARTS_LENGTH = 3;
        public const int VERIFICATION_CODE_LENGTH = 9;
        public const string BOT_TELEGRAM_TOKEN = "BOT_TELEGRAM_TOKEN";
        public const string REGEX_PROFILE_NAME_VALIDATION = "^[a-zA-Z0-9 ]+$";
        public const string REGEX_PROFILE_EMAIL_VALIDATION = @"^[a-zA-Z0-9._-]+(?<!\.)@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string REGEX_PHONE_NUMBER_VALIDATION = "^[0-9]+$";
        public const string REGEX_PASSWORD_ACCOUNT_VALIDATION = "^[a-zA-Z0-9_]+$";
        public const string REGEX_DOUBLE_POINTS = "\\.\\.+";
        public static string BOT_TELEGRAM_URI = $"https://api.telegram.org/bot{BOT_TELEGRAM_TOKEN}/";
        public const string STRING_CHAR_SPACE = " ";
        public const char STRING_CHAR_ARROBA = '@';
        public const string STRING_CHAR_DASH = "-";
        public const string STRING_CHAR_POINT = ".";
        public const string LOGGER_START_INFO = "Start";
        public const string LOGGER_FILE_DIRECTORY = "../../../logs/catanserver-log-.txt";
        public const string URI_SERVICE_EMPTY = "Empty service URI";
        public const string LANGUAGE_ENGLISH_FORMAT_RESX = "en";
        public const string LANGUAGE_ESPANISH_FORMAT_RESX = "es";
        public const string URI_GAMESERVICE_DIRECTION = "CatanService.EndPoint.GameServiceEndpoint";
        public const string URI_ACCOUNTSERVICE_DIRECTION = "CatanService.EndPoint.AccountServiceEndPoint";
        //public const string REGEX_NAME_USER = 
        public static string MessageDataBaseUnableToLoad(string language, Exception ex)
        {
            Log.Error(ex.ToString());
            return Resources.ResourceManager.GetString("dialog_database_load_exception", new CultureInfo(language));
        }
        public static string MessageDataBaseUnableToLoad(string language)
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
        public static string MessageServerLostConnection(string language)
        {
            return Resources.ResourceManager.GetString("dialog_connection_server_exception", new CultureInfo(language));
        }
        public static string MessageTokenSubject(string language)
        {
            return Resources.ResourceManager.GetString("subject_verification_code", new CultureInfo(language));
        }
        public static string MessageSuccess(string language)
        {
            return Resources.ResourceManager.GetString("dialog_success", new CultureInfo(language));
        }
        public static string MessageUnableToSaveData(string language)
        {
            return Resources.ResourceManager.GetString("dialog_database_save_exception", new CultureInfo(language));
        }
        public static string GetAPITelegramMessageURI(string chatId, string message)
        {
            return $"sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}";
        }
        public static string GetBaseAddressServiceTelegramBot(string token)
        {
            return $"https://api.telegram.org/bot{token}/";
        }

    }
}
