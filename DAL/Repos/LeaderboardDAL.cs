using Shared.Mappers;
using DTO.Dtos;
using DTO.Interfaces;

namespace DAL.Repos
{
    public class LeaderboardDAL : ILeaderboardDAL
    {
        private readonly AppDbContext _context;
        
        public LeaderboardDAL(AppDbContext context)
        {
            _context = context;
        }


        public LeaderboardDto CreateLeaderboard(LeaderboardDto leaderboard)
        {
            if (leaderboard == null)
                throw new ArgumentNullException(nameof(leaderboard));

            var entity = LeaderboardMapper.FromLeaderboardDto(leaderboard);
            
            _context.Leaderboards.Add(entity);
            _context.SaveChanges();
            
            return LeaderboardMapper.ToLeaderboardDto(entity);
        }

        public LeaderboardDto ReadLeaderboard(int id)
        {
            var entity = _context.Leaderboards.Find(id);
            
            return  LeaderboardMapper.ToLeaderboardDto(entity);
        }

        public void UpdateLeaderboard(LeaderboardDto leaderboard)
        {
            if (leaderboard == null)
                throw new ArgumentNullException(nameof(leaderboard));

            var existingEntity = _context.Leaderboards.Find(leaderboard.LeaderboardId);

            if (existingEntity == null)
                throw new Exception($"No leaderboard found with ID {leaderboard.LeaderboardId}");
            
            existingEntity.Name = leaderboard.Name;
            existingEntity.LeaderboardId = leaderboard.LeaderboardId;
            existingEntity.UserId = leaderboard.UserId;
            
            _context.SaveChanges();
        }

        public void DeleteLeaderboard(int id)
        {
            var entity = _context.Leaderboards.Find(id);
            
            if (entity == null)
                throw new Exception($"No leaderboard found with ID {id}");
            
            _context.Leaderboards.Remove(entity);
            _context.SaveChanges();
        }

        public List<LeaderboardDto> GetAllLeaderboards()
        {
            throw new NotImplementedException();
        }
    }
}
