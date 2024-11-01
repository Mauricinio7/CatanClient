using CatanClient.Services;
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
        public ObservableCollection<PlayerScore> FriendScores { get; set; }
        public ObservableCollection<PlayerScore> WorldScores { get; set; }
        public ObservableCollection<PlayerScore> WeeklyScores { get; set; }


        public ScoreboardViewModel(ServiceManager service)
        {
           this.serviceManager = service;
            FriendScores = new ObservableCollection<PlayerScore>();
            WorldScores = new ObservableCollection<PlayerScore>();
            WeeklyScores = new ObservableCollection<PlayerScore>();

        }


        public void LoadFriendScores()
        {
            FriendScores.Clear();
            FriendScores.Add(new PlayerScore { Position = 1, PlayerName = "Mauricinio7", GamesWon = 5, TotalPoints = 65 });
            FriendScores.Add(new PlayerScore { Position = 2, PlayerName = "TonyGamer", GamesWon = 4, TotalPoints = 56 });
            FriendScores.Add(new PlayerScore { Position = 3, PlayerName = "XxGamerxX", GamesWon = 2, TotalPoints = 87 });
            FriendScores.Add(new PlayerScore { Position = 4, PlayerName = "Player 93", GamesWon = 0, TotalPoints = 10 });
        }

        public void LoadWorldScores()
        {
            WorldScores.Clear();
            WorldScores.Add(new PlayerScore { Position = 1, PlayerName = "GlobalPlayer1", GamesWon = 10, TotalPoints = 150 });
            WorldScores.Add(new PlayerScore { Position = 2, PlayerName = "GlobalPlayer2", GamesWon = 8, TotalPoints = 120 });
            WorldScores.Add(new PlayerScore { Position = 3, PlayerName = "GlobalPlayer3", GamesWon = 6, TotalPoints = 100 });
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
