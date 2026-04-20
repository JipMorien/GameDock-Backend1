using DTO.Interfaces;
using Domain;
using Shared.Mappers;

namespace BLL.Containers
{
    public class LeaderboardContainer
    {
        private readonly ILeaderboardDAL _leaderboardDAL;

        public LeaderboardContainer(ILeaderboardDAL leaderboardDAL)
        {
            _leaderboardDAL = leaderboardDAL ?? throw new ArgumentNullException(nameof(leaderboardDAL));
        }

        public void CheckLeaderboard(Leaderboard leaderboard)
        {
            if (leaderboard == null)
                throw new ArgumentNullException(nameof(leaderboard));
        
            if (leaderboard.LeaderboardId < 0)
                throw new ArgumentException("Leaderboard ID cannot be less than 0");
            if (string.IsNullOrEmpty(leaderboard.Name))
                throw new ArgumentException("Leaderboard name cannot be empty");
            if (leaderboard.UserId < 0)
                throw new ArgumentException("User ID cannot be less than 0");
        }

        public Leaderboard CreateLeaderboard(Leaderboard leaderboard)
        {
            CheckLeaderboard(leaderboard);
            
            var newLeaderboardDto = _leaderboardDAL.CreateLeaderboard(LeaderboardMapper.ToLeaderboardDto(leaderboard));
            
            if(newLeaderboardDto == null)
                throw new Exception("Leaderboard could not be created");
            
            return LeaderboardMapper.FromLeaderboardDto(newLeaderboardDto);
        }

        public Leaderboard ReadLeaderboard(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID can't be negative");

            var leaderboardDto = _leaderboardDAL.ReadLeaderboard(id);
            
            if (leaderboardDto == null)
                throw new ArgumentException("Leaderboard could not be read");
            
            return LeaderboardMapper.FromLeaderboardDto(leaderboardDto);
        }

        public void UpdateLeaderboard(Leaderboard leaderboard)
        {
            CheckLeaderboard(leaderboard);
            
            var existingLeaderboard = _leaderboardDAL.ReadLeaderboard(leaderboard.LeaderboardId);
            if (existingLeaderboard == null)
                throw new ArgumentException("Leaderboard could not be read");
            
            _leaderboardDAL.UpdateLeaderboard(LeaderboardMapper.ToLeaderboardDto(leaderboard));
        }

        public void DeleteLeaderboard(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID can't be negative");
            
            var existingLeaderboard = _leaderboardDAL.ReadLeaderboard(id);
            if (existingLeaderboard == null)
                throw new ArgumentException("Leaderboard could not be read");
            
            _leaderboardDAL.DeleteLeaderboard(id);
        }

        public List<Leaderboard> GetAllLeaderboards()
        {
            var leaderboardDtos = _leaderboardDAL.GetAllLeaderboards();
            
            return leaderboardDtos.Select(LeaderboardMapper.FromLeaderboardDto).ToList();
        }
    
    }
}

