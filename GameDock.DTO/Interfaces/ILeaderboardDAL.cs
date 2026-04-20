using GameDock.DTO.Dtos;

namespace GameDock.DTO.Interfaces
{
    public interface ILeaderboardDAL
    {
        LeaderboardDto CreateLeaderboard(LeaderboardDto leaderboard);
        LeaderboardDto? ReadLeaderboard(int id);
        void UpdateLeaderboard(LeaderboardDto leaderboard);
        void DeleteLeaderboard(int id);
        List<LeaderboardDto> GetAllLeaderboards();

    }
}