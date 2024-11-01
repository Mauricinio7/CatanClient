﻿using CatanClient.Properties;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public const string REGEX_PASSWORD_ACCOUNT_VALIDATION = "^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?\":{}|<>_])(?=.*[a-zA-Z0-9]).{8,}$";
        public const string REGEX_DOUBLE_POINTS = "\\.\\.+";
        public const string STRING_CHAR_SPACE = " ";
        public const char STRING_CHAR_ARROBA = '@';
        public const string STRING_CHAR_DASH = "-";
        public const string STRING_CHAR_POINT = ".";
        public const string LOGGER_FILE_DIRECTORY = "C:/Users/mauricio/source/repos/CatanClient/logs/errorlog.txt";
        public const string LANGUAGE_ENGLISH_FORMAT_RESX = "en";
        public const string LANGUAGE_ESPANISH_FORMAT_RESX = "es";
        public const string IPACCOUNTSERVICE = "http://10.144.41.3:8181/AccountService";
        public const string IP_GAME_SERVICE = "http://10.144.41.3:8191/GameService";
        public const string IP_CHAT_SERVICE = "net.tcp://10.144.41.3:8202/ChatService";
        public const string IP_PROFILE_SERVICE = "http://10.144.41.3:8383/ProfileService";
        public const string IP_GUEST_ACCOUNT_SERVICE = "http://10.144.41.3:8484/GuestAccountService";
        public const string FADEOUTANIMATION = "FadeOutAnimation";
        public const string SHOWMAINMENUBACKGROUND = "ShowMainMenuBackgroundView";
        public const string SHOWMAINMENU = "ShowMainMenuView";
        public const string SHOW_GUEST_MAIN_MENU = "ShowGuestMainMenuView";
        public const string SHOWVERIFYACCOUNT = "ShowVerifyAccountView";
        public const string OCULTVERIFYACCOUNT = "OcultVerifyAccountView";
        public const string SHOWGAMELOBBY = "ShowGameLobbyView";
        public const string SHOWCONFIGUREPROFILE = "ShowConfigureProfile";
        public const string BACK_TO_MAIN_MENU_ROOM = "BackToMainMenuRoom";
        public const string BACK_TO_GUEST_MAIN_MENU_ROOM = "BackToGuestMainMenuRoom";
        public const string CLOSE_EDIT_PROFILE = "EditProfileWindow_Close";
        public const string CLOSE_VERIFY_ACCOUNT_CHANGE = "VerifyAccountChangeWindow_Close";
        public const string CLOSE_EDIT_PASSWORD = "EditPasswordWindow_Close";
        public const string RECIVEMESSAGE = "ReceiveMessage";
        public const string ANIMATEDGRID = "animatedGrid";
        public const string FADEINANIMATION = "FadeInAnimation";
        public const string SLIDEOUTFROMTOPANIMATION = "SlideOutFromTopAnimation";
        public const string SLIDEOUTFROMRIGHTANIMATION = "SlideOutFromRightAnimation";
        public const string SLIDEOUTFROMLEFTANIMATION = "SlideOutFromLeftAnimation";
        public const string SLIDE_OUT_FROM_RIGHT_ANIMATION_PANELS = "SlideOutFromRightAnimationForPanels";
        public const string SYSTEM_NAME = "System";
        public const string LABEL_STYLE = "LabelStyle";
        public const string TEXT = "Text";
        public const string PLACEHOLDER_TEXT = "PlaceholderText";
        public const string FONT_SIZE_VALUE = "FontSizeValue";
        public const string BACKGROUND_COLOR_VALUE = "BackgroundColorValue";
        public const string PLACEHOLDER_FOREGROUND_COLOR_VALUE = "PlaceholderForegroundColorValue";
        public const string WRITING_FOREGROUND_COLOR_VALUE = "WritingForegroundColorValue";
        public const string PADDING_VALUE = "PaddingValue";
        public const string BORDER_THICKNESS_VALUE = "BorderThicknessValue";
        public const string WIDTH = "Width";
        public const string HEIGHT = "Height";
        public const string PASSWORD = "Password";
        public const string EMAIL = "Email";
        public const string PHONE = "Phone";
        public const string USERNAME = "Username";
        public const string TEXT_BOX_WIDTH = "TextBoxWidth";
        public const string TEXT_BOX_HEIGHT = "TextBoxHeight";
        public const string CHECK_BOX_MARGIN = "CheckBoxMargin";
        public const string SYNTH_WAVE_BACKGROUND2_PATH = "pack://application:,,,/Resources/Gifs/SynthWaveAnimatedBackground2.gif";
        public const string SYNTH_WAVE_BACKGROUND1_PATH = "pack://application:,,,/Resources/Gifs/AnimatedBackground1.gif";
        public const string IMAGE_FILTER = "Image Files (*.jpg;*.png)|*.jpg;*.png";
        public const string PROFILE_IMAGE_DIRECTORY = "ProfilePhotos";


        public static string ProfilePhotoPath(int Id)
        {
            return $"ProfilePhoto{Id}.jpg";
        }
        public static string MessageDataBaseUnableToLoad(string language, Exception ex)
        {
            Log.Information(ex.Message);
            return Resources.ResourceManager.GetString("dialog_database_load_exception", new CultureInfo(language));
        }

        public static string MessageDataBaseUnableToLoad(string language)
        {                                           
            return Resources.ResourceManager.GetString("dialog_database_load_exception_message", new CultureInfo(language));
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

        public static string TitleImageSelector(string language)
        {
            return Resources.ResourceManager.GetString("image_selector_title", new CultureInfo(language));
        }

        public static string MessageTooBigFile(string language)
        {
            return Resources.ResourceManager.GetString("dialog_too_big_file_message", new CultureInfo(language));
        }
        public static string MessageGameNotFound(string language)
        {
            return Resources.ResourceManager.GetString("dialog_game_not_found_message", new CultureInfo(language));
        }
        public static string MessageRestartGame(string language)
        {
            return Resources.ResourceManager.GetString("dialog_restart_message", new CultureInfo(language));
        }
        public static string TittleRestart(string language)
        {
            return Resources.ResourceManager.GetString("tittle_restart", new CultureInfo(language));
        }
        public static string MessageFailVerifyUser(string language)
        {
            return Resources.ResourceManager.GetString("dialog_fail_verify_user_message", new CultureInfo(language));
        }
        public static string MessageUnableToSaveData(string language)
        {
            return Resources.ResourceManager.GetString("dialog_database_save_exception_message", new CultureInfo(language));
        }

        public static string TitleUnableToSaveData(string language)
        {
            return Resources.ResourceManager.GetString("dialog_database_save_exception", new CultureInfo(language));
        }

        public static string MessageUnableToLoadData(string language)
        {
            return Resources.ResourceManager.GetString("dialog_database_load_exception_message", new CultureInfo(language));
        }

        public static string TitleUnableToLoadData(string language)
        {
            return Resources.ResourceManager.GetString("dialog_database_load_exception", new CultureInfo(language));
        }
        public static string MessageChangeUsernameSucces(string language)
        {
            return Resources.ResourceManager.GetString("dialog_change_username_succes_message", new CultureInfo(language));
        }
        public static string MessageChangeUsernameFail(string language)
        {
            return Resources.ResourceManager.GetString("dialog_change_username_fail_message", new CultureInfo(language));
        }

        public static string TitleChangeUsername(string language)
        {
            return Resources.ResourceManager.GetString("dialog_change_username", new CultureInfo(language));
        }
        public static string EnterNew(string language)
        {
            return Resources.ResourceManager.GetString("label_enter_new", new CultureInfo(language));
        }
        public static string Email(string language)
        {
            return Resources.ResourceManager.GetString("label_email", new CultureInfo(language));
        }
        public static string Username(string language)
        {
            return Resources.ResourceManager.GetString("label_username", new CultureInfo(language));
        }
        public static string PhoneNumber(string language)
        {
            return Resources.ResourceManager.GetString("label_phone", new CultureInfo(language));
        }
        public static string DialogWelcome(string language)
        {
            return Resources.ResourceManager.GetString("dialog_welcome", new CultureInfo(language));
        }
        public static string MessagePasswordNotMacth(string language)
        {
            return Resources.ResourceManager.GetString("dialog_password_dont_match_message", new CultureInfo(language));
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

        public static void ShowMessgeServerLost()
        {
            MessageBox.Show(Utilities.MessageServerLostConnection(CultureInfo.CurrentCulture.Name), Utilities.TittleServerLostConnection(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void ShowMessageDataBaseUnableToLoad()
        {
            MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
