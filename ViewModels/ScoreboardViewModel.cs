using CatanClient.GameService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.ViewModels
{
    internal class ScoreboardViewModel : ViewModelBase
    {
        private readonly ServiceManager serviceManager;
        private readonly AccountService.ProfileDto profile;
        public ObservableCollection<PlayerScore> FriendScores { get; set; }
        public ObservableCollection<PlayerScore> WorldScores { get; set; }
        public ObservableCollection<PlayerScore> WeeklyScores { get; set; }


        public ScoreboardViewModel(ServiceManager service)
        {
            this.serviceManager = service;
            profile = serviceManager.ProfileSingleton.Profile;
            FriendScores = new ObservableCollection<PlayerScore>();
            WorldScores = new ObservableCollection<PlayerScore>();
            WeeklyScores = new ObservableCollection<PlayerScore>();

        }


        public void LoadFriendScores()
        {

            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                OperationResultListScoreGame result = await serviceManager.GameServiceClient.GetScoreboardFriends(AccountUtilities.CastAccountProfileToGameService(profile));

                if (result.IsSuccess)
                {
                    FriendScores.Clear();
                    foreach (var score in result.ListProfileScoreDto)
                    {
                        FriendScores.Add(new PlayerScore
                        {
                            Position = score.Position,
                            PlayerName = score.Name,
                            //GamesWon = score.GamesWon,
                            TotalPoints = score.Score
                        });
                    }
                }
                else
                {
                    Utilities.ShowMessageDataBaseUnableToLoad();
                }
            });
        }

        public void LoadWorldScores()
        {

            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                OperationResultListScoreGame result = await serviceManager.GameServiceClient.GetScoreboardWorld(AccountUtilities.CastAccountProfileToGameService(profile));

                if (result.IsSuccess)
                {
                    WorldScores.Clear();
                    foreach (var score in result.ListProfileScoreDto)
                    {
                        WorldScores.Add(new PlayerScore
                        {
                            Position = score.Position,
                            PlayerName = score.Name,
                            //GamesWon = score.GamesWon,
                            TotalPoints = score.Score
                        });
                    }
                }
                else
                {
                    Utilities.ShowMessageDataBaseUnableToLoad();
                }
            });
        }

        public void LoadWeeklyScores()
        {
            WeeklyScores.Clear();
            WeeklyScores.Add(new PlayerScore { Position = 1, PlayerName = "WeeklyPlayer1", GamesWon = 2, TotalPoints = 30 });
            WeeklyScores.Add(new PlayerScore { Position = 2, PlayerName = "WeeklyPlayer2", GamesWon = 1, TotalPoints = 15 });
            WeeklyScores.Add(new PlayerScore { Position = 3, PlayerName = "WeeklyPlayer3", GamesWon = 1, TotalPoints = 10 });
        }

        public class PlayerScore
        {
            public int Position { get; set; }
            public string PlayerName { get; set; }
            public int GamesWon { get; set; }
            public int TotalPoints { get; set; }
        }

    }
}
