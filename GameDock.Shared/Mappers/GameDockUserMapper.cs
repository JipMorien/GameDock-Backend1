using Domain;
using DTO.Dtos;

namespace Shared.Mappers
{
    public static class GameDockUserMapper
    {
        public static GameDockUser FromUserDto(GameDockUserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new GameDockUser(
                dto.IsAdmin,
                dto.GameDockUserId,
                dto.UserName,
                dto.Email,
                dto.PasswordHash,
                dto.CreatedAt);
        }

        public static GameDockUserDto ToUserDto(GameDockUser gameDockUser)
        {
            if (gameDockUser == null)
                throw new ArgumentNullException(nameof(gameDockUser));

            return new GameDockUserDto
            {
                IsAdmin = gameDockUser.IsAdmin,
                GameDockUserId = gameDockUser.GameDockUserId,
                UserName = gameDockUser.UserName,
                Email = gameDockUser.Email,
                PasswordHash = gameDockUser.PasswordHash,
                CreatedAt = gameDockUser.CreatedAt
            };
        }
    }
}