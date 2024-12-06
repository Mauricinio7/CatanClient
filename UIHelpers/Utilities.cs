using CatanClient.Gameplay.Helpers;
using CatanClient.Properties;
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
    internal static class Utilities
    {
        public const string DEFAULT_LANGUAGE = "en";
        public const string ALPHABET_EN = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const int VERIFICATION_CODE_PARTS_LENGTH = 3;
        public const int VERIFICATION_CODE_LENGTH = 9;
        public const string REGEX_PROFILE_NAME_VALIDATION = "^[a-zA-ZñÑ0-9]+( [a-zA-ZñÑ0-9]+)*$";
        public const string REGEX_PROFILE_EMAIL_VALIDATION = @"^[a-zA-ZñÑ0-9._-]+(?<!\.)@[a-zA-ZñÑ0-9.-]+\.[a-zA-ZñÑ]{2,}$";
        public const string REGEX_PHONE_NUMBER_VALIDATION = @"^\+[0-9]+$";
        public const string REGEX_PASSWORD_ACCOUNT_VALIDATION = "^(?=.*[A-ZÑ])(?=.*[0-9])(?=.*[!@#$%^&*(),.?\":{}|<>_])(?=.*[a-zA-ZñÑ0-9]).{8,}$";
        public const int USERNAME_MIN_LENGTH = 8;
        public const int USERNAME_MAX_LENGTH = 15;
        public const int FIEL_MAX_LENGTH = 15;
        public const string REGEX_DOUBLE_POINTS = "\\.\\.+";
        public const string STRING_CHAR_SPACE = " ";
        public const char STRING_CHAR_ARROBA = '@';
        public const string STRING_CHAR_DASH = "-";
        public const string STRING_CHAR_POINT = ".";
        public const string LOGGER_FILE_NAME = "log.txt";
        public const string LOGGER_FOLDER = "logs";
        public const string DEFAULT_PHOTO_PATH = "pack://application:,,,/Resources/Images/ShibaTest.png";
        public const string LANGUAGE_ENGLISH_FORMAT_RESX = "en";
        public const string LANGUAGE_ESPANISH_FORMAT_RESX = "es";
        public const string IP_ACCOUNT_SERVICE = "net.tcp://192.168.58.207:8181/AccountService";
        public const string IP_GAME_SERVICE = "net.tcp://192.168.58.207:8192/GameService";
        public const string IP_CHAT_SERVICE = "net.tcp://192.168.58.207:8202/ChatService";
        public const string IP_PROFILE_SERVICE = "net.tcp://192.168.58.207:8383/ProfileService";
        public const string IP_GUEST_ACCOUNT_SERVICE = "net.tcp://192.168.58.207:8484/GuestAccountService";
        public const string FADE_OUT_ANIMATION = "FadeOutAnimation";
        public const string SHOW_MAIN_MENU_BACKGROUND = "ShowMainMenuBackgroundView";
        public const string SHOW_MAIN_MENU = "ShowMainMenuView";
        public const string SHOW_GUEST_MAIN_MENU = "ShowGuestMainMenuView";
        public const string SHOW_VERIFY_ACCOUNT = "ShowVerifyAccountView";
        public const string OCULT_VERIFY_ACCOUNT = "OcultVerifyAccountView";
        public const string SHOW_GAME_LOBBY = "ShowGameLobbyView";
        public const string GET_GAME_FOR_SCREEN = "GetGameForScreen";
        public const string SHOW_CONFIGURE_PROFILE = "ShowConfigureProfile";
        public const string SHOW_INVITE_FRIENDS = "ShowInviteFriendsView";
        public const string HIDE_INVITE_FRIENDS = "HideInviteFriendsView";
        public const string SHOW_CHANGE_FORGOT_PASSWORD = "ShowChangeForgotPasswordView";
        public const string BACK_TO_MAIN_MENU_ROOM = "BackToMainMenuRoom";
        public const string BACK_TO_GUEST_MAIN_MENU_ROOM = "BackToGuestMainMenuRoom";
        public const string CLOSE_EDIT_PROFILE = "EditProfileWindow_Close";
        public const string CLOSE_VERIFY_ACCOUNT_CHANGE = "VerifyAccountChangeWindow_Close";
        public const string CLOSE_EDIT_PASSWORD = "EditPasswordWindow_Close";
        public const string SHOW_LOADING_SCREEN = "ShowLoadingScreen";
        public const string HIDE_LOADING_SCREEN = "HideLoadingScreen";
        public const string SHOW_GAME_SCREEN = "ShowGameScreen";
        public const string SHOW_ROLL_DICE_ANIMATION = "ShowRollDiceAnimation";
        public const string SHOW_WIN_ANIMATION = "ShowWinAnimation";
        public const string SHOW_DICE_RESULT_ANIMATION = "ShowDiceResultAnimation";
        public const string RECIVE_MESSAGE = "ReceiveMessage";
        public const string RECIVE_MESSAGE_GAME = "ReceiveMessageGame";
        public const string LOAD_PLAYER_LIST = "LoadPlayerList";
        public const string LOAD_GAME_PLAYER_LIST = "LoadGamePlayerList";
        public const string LOAD_GAME_PLAYER_BOARD = "LoadGamePlayerBoard";
        public const string UPDATE_GAME_PLAYER_BOARD = "UpdateGamePlayerBoard";
        public const string UPDATE_PLAYER_RESOURCES = "UpdatePlayerResources";
        public const string ANIMATED_GRID = "animatedGrid";
        public const string FADE_IN_ANIMATION = "FadeInAnimation";
        public const string SLIDE_OUT_FROM_TOP_ANIMATION = "SLIDE_OUT_FROM_TOP_ANIMATION";
        public const string SLIDE_OUT_FROM_RIGHT_ANIMATION = "SlideOutFromRightAnimation";
        public const string SLIDE_OUT_FROM_LEFT_ANIMATION = "SlideOutFromLeftAnimation";
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
        public const string DICE_ROLL_ANIMATION = "pack://application:,,,/Gameplay/Resources/Animations/dice_roll_animation_dual.gif";
        public const string WIN_ANIMATION = "pack://application:,,,/Gameplay/Resources/Animations/WinAnimation1.gif";
        public const string IMAGE_FILTER = "Image Files (*.jpg;*.png)|*.jpg;*.png";
        public const string PROFILE_IMAGE_DIRECTORY = "ProfilePhotos";
        public const string PROFILE = "profile";
        public const string POINTS = "points";
        public const string ACCES_KEY = "accesKey";
        public const string GAME = "game";
        public const string SENDER_PROFILE = "senderProfile";
        public const string CLOSE_EXPEL_PLAYER = "CloseExpelPlayer";
        public const string CLOSE_KICK_PLAYER = "CloseKickPlayer";
        public const string UPDATE_TIME = "UpdateTime";
        public const string UPDATE_TIME_GAME = "UpdateTimeGame";
        public const string TURN = "turn";
        public const string UPDATE_GAME_ADMIN = "UpdateGameAdmin";
        public const string LUNAR_STONE_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/LunarStone.png";
        public const string TRITONIUM_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/TritoniumWood.png";
        public const string ALPHA_NANOFIBERS_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/AlphaNanofibers.png";
        public const string EPSILON_BIOMASS_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/EpsilonGrain.png";
        public const string GRX_810_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/GRX-810Stone.png";
        public const string LUNAR_STONE_BIOME_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/LunarStoneBiomec.png";
        public const string TRITONIUM_BIOME_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/WoodBiomec.png";
        public const string ALPHA_NANOFIBERS_BIOME_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/FiberBiomec.png";
        public const string EPSILON_BIOMASS_BIOME_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/BiomasaBiomec.png";
        public const string GRX_810_BIOME_IMAGE_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/Biomes/GRX-81Biomec.png";
        public const string CITY_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/City.png";
        public const string TOWN_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/Town.png";
        public const string ROAD_PATH = "pack://application:,,,/Gameplay/Resources/Images/GameResources/Road.png";
        public const string LUNAR_STONE = "Lunar Stone";
        public const string TRITONIUM = "Tritonium";
        public const string ALPHA_NANOFIBERS = "Alpha Nanofibers";
        public const string EPSILON_BIOMASS = "Epsilon Biomass";
        public const string GRX_810 = "GRX-810";
        public const string CITY = "City";
        public const string SETTLEMENT = "Settlement";
        public const string ROAD = "Road";
        public const string TIME_FORMAT = "mm\\:ss";
        public const string LOAD_GAME_TRADE = "LoadGameTrade";
        public const string HIDE_TRADE_CONTROL = "HideTradeControl";
        public const string FRIENDS = "Friends"; 
        public const string WORLD = "World";
        public const string LOGIN_MUSIC = "pack://application:,,,/Resources/Sounds/LoginMusic.wav";
        public const string MAIN_MENU_MUSIC = "pack://application:,,,/Resources/Sounds/MainMenuMusic.wav";
        public const string SHOW_SCORE_FRAME = "ShowScoreFrame";
        public const string HIDE_FRIENDS = "HideFriends";




        public static string ProfilePhotoPath(int Id)
        {
            return $"ProfilePhoto{Id}.jpg";
        }
        public static string ProfilePhotoPathWithVersion(int Id, int version)
        {
            return $"ProfilePhoto{Id}_V{version}.jpg";
        }
        public static string ProfilePhotoPathDeleteVersion(int Id)
        {
            return $"ProfilePhoto{Id}_V*.jpg";
        }
        public static string GetDefaultPhotoPath()
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "ShibaTest.png");
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
        public static string MessageGameExpel(string language)
        {
            return Resources.ResourceManager.GetString("global_game_expel", new CultureInfo(language));
        }
        public static string MessageSuccesRemoveFriend(string language)
        {
            return Resources.ResourceManager.GetString("dialog_succes_remove_friend_message", new CultureInfo(language));
        }
        public static string MessageSuccesAddFriend(string language)
        {
            return Resources.ResourceManager.GetString("dialog_succes_add_friend_message", new CultureInfo(language));
        }
        public static string MessageSuccesRejectFriend(string language)
        {
            return Resources.ResourceManager.GetString("dialog_succes_reject_friend_message", new CultureInfo(language));
        }
        public static string MessageSuccesInviteFriend(string language)
        {
            return Resources.ResourceManager.GetString("dialog_invitation_message", new CultureInfo(language));
        }
        public static string MessageSuccesFriendRequest(string language)
        {
            return Resources.ResourceManager.GetString("dialog_succes_friend_request_message", new CultureInfo(language));
        }
        public static string MessageSuccesPasswordChange(string language)
        {
            return Resources.ResourceManager.GetString("dialog_password_changed_message", new CultureInfo(language));
        }
        public static string MessageSuccesKickPlayer(string language)
        {
            return Resources.ResourceManager.GetString("dialog_succes_kick_player_message", new CultureInfo(language));
        }
        public static string MessageSuccesSendVerificationCode(string language)
        {
            return Resources.ResourceManager.GetString("dialog_succes_send_code_message", new CultureInfo(language));
        }
        public static string TitleVerifyAccount(string language)
        {
            return Resources.ResourceManager.GetString("dialog_verify_account", new CultureInfo(language));
        }
        public static string MessageAccountInUse(string language)
        {
            return Resources.ResourceManager.GetString("dialog_account_in_use", new CultureInfo(language));
        }
        public static string MessageNameInUse(string language)
        {
            return Resources.ResourceManager.GetString("dialog_name_in_use", new CultureInfo(language));
        }
        public static string MessageSuccessfulBuild(string language)
        {
            return Resources.ResourceManager.GetString("dialog_successful_build_message", new CultureInfo(language));
        }
        public static string MessageCannotHaveOnlyBlankSpace(string language)
        {
            return Resources.ResourceManager.GetString("dialog_only_blanck_spaces_message", new CultureInfo(language));
        }
        public static string MessageCannontAddDeletedFriend(string language)
        {

           return Resources.ResourceManager.GetString("dialog_cant_add_deleted_friend_message", new CultureInfo(language));
        }
        public static string MessageTooLargeInput(string language)
        {

            return Resources.ResourceManager.GetString("dialog_exceed_character_limit_message", new CultureInfo(language));
        }

        public static string MessageUnsuccessfulBuild(string language)
        {
            return Resources.ResourceManager.GetString("dialog_unsuccessful_build_message", new CultureInfo(language));
        }
        public static string MessageGameEndNoPlayers(string language)
        {
            return Resources.ResourceManager.GetString("dialog_game_end_no_players_message", new CultureInfo(language));
        }
        public static string MessageTradingError(string language)
        {
            return Resources.ResourceManager.GetString("dialog_error_trading_message", new CultureInfo(language));
        }
        public static string MessageTradingSucces(string language)
        {
            return Resources.ResourceManager.GetString("dialog_succes_trade_message", new CultureInfo(language));
        }
        public static string MessageSendTradeSucces(string language)
        {
            return Resources.ResourceManager.GetString("dialog_succes_send_trade_message", new CultureInfo(language));
        }
        public static string TitleGameEnd(string language)
        {
            return Resources.ResourceManager.GetString("dialog_game_end_title", new CultureInfo(language));
        }
        public static string MessageGameExpelDialog(string language)
        {
            return Resources.ResourceManager.GetString("dialog_game_expel_message", new CultureInfo(language));
        }
        public static string MessageVoteRegistered(string language)
        {
            return Resources.ResourceManager.GetString("dialog_vote_registered_message", new CultureInfo(language));
        }
        public static string TitleBuild(string language)
        {
            return Resources.ResourceManager.GetString("dialog_build", new CultureInfo(language));
        }
        public static string MessageExistsFriendRequest(string language)
        {
            return Resources.ResourceManager.GetString("dialog_exists_friend_request_message", new CultureInfo(language));
        }
        public static string MessageNotFoundProfile(string language)
        {
            return Resources.ResourceManager.GetString("dialog_not_found_profile_message", new CultureInfo(language));
        }
        public static string GlobalReady(string language)
        {
            return Resources.ResourceManager.GetString("global_ready", new CultureInfo(language));
        }
        public static string GlobalNoReady(string language)
        {
            return Resources.ResourceManager.GetString("global_no_ready", new CultureInfo(language));
        }
        public static string OptionBadConduct(string language)
        {
            return Resources.ResourceManager.GetString("option_bad_conduct", new CultureInfo(language));
        }
        public static string OptionInactivity(string language)
        {
            return Resources.ResourceManager.GetString("option_inactivity", new CultureInfo(language));
        }
        public static string OptionOffensiveLanguage(string language)
        {
            return Resources.ResourceManager.GetString("option_offensive_language", new CultureInfo(language));
        }
        public static string OptionExploits(string language)
        {
            return Resources.ResourceManager.GetString("option_exploits", new CultureInfo(language));
        }
        public static string LabelPoints(string language)
        {
            return Resources.ResourceManager.GetString("label_points", new CultureInfo(language));
        }
        public static string LabelWaitingPlayers(string language)
        {
            return Resources.ResourceManager.GetString("label_waiting_players", new CultureInfo(language));
        }
        public static string LabelJoiningGame(string language)
        {
            return Resources.ResourceManager.GetString("label_joining_game", new CultureInfo(language));
        }
        public static string LabelTimeRemaining(string language)
        {
            return Resources.ResourceManager.GetString("label_time_remaining", new CultureInfo(language));
        }
        public static string LogInfoStart(string language)
        {
            return Resources.ResourceManager.GetString("log_info_start", new CultureInfo(language));
        }
        public static string LogInfoEnd(string language)
        {
            return Resources.ResourceManager.GetString("log_info_end", new CultureInfo(language));
        }
        public static string GetTownText(string language)
        {
            return Resources.ResourceManager.GetString("global_town", new CultureInfo(language));
        }
        public static string GetCityText(string language)
        {
            return Resources.ResourceManager.GetString("global_city", new CultureInfo(language));
        }
        public static string GetRoadText(string language)
        {
            return Resources.ResourceManager.GetString("global_road", new CultureInfo(language));
        }
        public static string GetCancelText(string language)
        {
            return Resources.ResourceManager.GetString("button_cancel", new CultureInfo(language));
        }
        public static string GetTimeRemainingText(string language, string time)
        {
            string template = Resources.ResourceManager.GetString("label_time_remaining", new CultureInfo(language));
            return string.Format(template, time);
        }

        public static string GetAssigningTurnText(string language)
        {
            return Resources.ResourceManager.GetString("label_assigning_turn", new CultureInfo(language));
        }
        public static string GetGameStartedText(string language)
        {
            return Resources.ResourceManager.GetString("game_started", new CultureInfo(language));
        }
        public static string WinnerText(string language)
        {
            return Resources.ResourceManager.GetString("label_winner", new CultureInfo(language));
        }

        public static void ShowMessgeServerLost()
        {
            MessageBox.Show(Utilities.MessageServerLostConnection(CultureInfo.CurrentCulture.Name), Utilities.TittleServerLostConnection(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void ShowMessageDataBaseUnableToLoad()
        {
            MessageBox.Show(Utilities.MessageDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), Utilities.TittleDataBaseUnableToLoad(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void ShowMessageSuccessfulBuild()
        {
            MessageBox.Show(
                Utilities.MessageSuccessfulBuild(CultureInfo.CurrentCulture.Name),
                Utilities.TitleBuild(CultureInfo.CurrentCulture.Name),
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        public static void ShowMessageUnsuccessfulBuild()
        {
            MessageBox.Show(
                Utilities.MessageUnsuccessfulBuild(CultureInfo.CurrentCulture.Name),
                Utilities.TitleBuild(CultureInfo.CurrentCulture.Name),
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
        }

        public static void ShowMessageInvalidFileds()
        {
            MessageBox.Show(Utilities.MessageInvalidCaracters(CultureInfo.CurrentCulture.Name), Utilities.TittleInvalidCaracters(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void ShowMessageEmptyFields()
        {
            MessageBox.Show(Utilities.MessageEmptyField(CultureInfo.CurrentUICulture.Name), Utilities.TittleEmptyField(CultureInfo.CurrentUICulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
