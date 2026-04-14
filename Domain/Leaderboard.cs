namespace Domain
{
    public class Leaderboard
    {
        public int LeaderboardId {get; set;}
        public string Name {get; set;}
        public int UserId {get; set;}

        public Leaderboard(int leaderboardId, string name, int userId)
        {
            LeaderboardId = leaderboardId;
            Name = name;
            UserId = userId;
        }
    
    }
}
