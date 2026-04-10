using DTO.Dtos;
using Domain;

namespace BLL.Mappers
{
    public static class LeaderboardMapper
    {
        public static Leaderboard FromLeaderboardDto(LeaderboardDto dto)
        {
            if (dto == null) 
                throw new ArgumentNullException(nameof(dto));

            return new Leaderboard(
                dto.LeaderboardId,
                dto.Name,
                dto.UserId);
        }
        
        public static LeaderboardDto ToLeaderboardDto(Leaderboard leaderboard)
        {
            if (leaderboard == null) 
                throw new ArgumentNullException(nameof(leaderboard));

            return new LeaderboardDto
            {
                LeaderboardId = leaderboard.LeaderboardId,
                Name = leaderboard.Name,
                UserId = leaderboard.UserId
            };
        }
        
        
        
        
    
    }
}

