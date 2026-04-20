using DTO.Interfaces;
using Domain;
using Shared.Mappers;

namespace BLL.Containers
{
    public class GameDockUserContainer
    {
        private readonly IGameDockUserDAL _gameDockUserDAL;

        public GameDockUserContainer(IGameDockUserDAL gameDockUserDAL)
        {
            _gameDockUserDAL = gameDockUserDAL ?? throw new ArgumentNullException(nameof(gameDockUserDAL));
        }

        public void CheckGameDockUser(GameDockUser gameDockUser)
        {
            if (gameDockUser == null)
                throw new ArgumentNullException(nameof(gameDockUser));
            if (gameDockUser.GameDockUserId < 0)
                throw new ArgumentException("GameDockUserId cannot be negative.");
            if (string.IsNullOrWhiteSpace(gameDockUser.UserName))
                throw new ArgumentException("Username cannot be empty.");
            if (gameDockUser.UserName.Length > 50)
                throw new ArgumentException("Username cannot exceed 50 characters.");
            if (string.IsNullOrWhiteSpace(gameDockUser.Email))
                throw new ArgumentException("Email cannot be empty.");
            if (!gameDockUser.Email.Contains("@"))
                throw new ArgumentException("Email must be valid.");
            if (string.IsNullOrWhiteSpace(gameDockUser.PasswordHash))
                throw new ArgumentException("PasswordHash cannot be empty.");
            if (gameDockUser.CreatedAt == default)
                throw new ArgumentException("CreatedAt must be valid.");
        }

        public GameDockUser CreateUser(GameDockUser gameDockUser)
        {
            CheckGameDockUser(gameDockUser);
            
            var newGameDockUserDto = _gameDockUserDAL.CreateUser(GameDockUserMapper.ToUserDto(gameDockUser));
            
            if(newGameDockUserDto == null)
                throw new ArgumentException("GameDockUserDto cannot be null");
            
            return GameDockUserMapper.FromUserDto(newGameDockUserDto);
        }

        public GameDockUser ReadUser(int id)
        {
            if (id <= 0)    
                throw new ArgumentException("ID cannot be negative.");
            
            var gameDockUserDto = _gameDockUserDAL.ReadUser(id);
            
            if (gameDockUserDto == null)
                throw new ArgumentException("GameDockUserDto cannot be null");
            
            return GameDockUserMapper.FromUserDto(gameDockUserDto);
        }

        public void UpdateUser(GameDockUser gameDockUser)
        {
            CheckGameDockUser(gameDockUser);

            var existingGameDockUser = _gameDockUserDAL.ReadUser(gameDockUser.GameDockUserId);
            if (existingGameDockUser == null)
                throw new ArgumentException("GameDockUserDto could not be read");
            
            _gameDockUserDAL.UpdateUser(GameDockUserMapper.ToUserDto(gameDockUser));
        }

        public void DeleteUser(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be negative.");
            
            var existingGameDockUser = _gameDockUserDAL.ReadUser(id);
            if (existingGameDockUser == null)
                throw new ArgumentException("GameDockUserDto could not be read");

            _gameDockUserDAL.DeleteUser(id);
        }

        public List<GameDockUser> GetAllUsers()
        {
            var gameDockUserDtos = _gameDockUserDAL.GetAllUsers();
            
            return gameDockUserDtos.Select(GameDockUserMapper.FromUserDto).ToList();
        }
    }
    
}

