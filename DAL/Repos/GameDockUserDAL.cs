using Shared.Mappers;
using DTO.Dtos;
using DTO.Interfaces;

namespace DAL.Repos
{
    public class GameDockUserDAL : IGameDockUserDAL
    {

        private readonly AppDbContext _context;
        
        public GameDockUserDAL(AppDbContext context)
        {
            _context = context;
        }

        public GameDockUserDto CreateUser(GameDockUserDto gameDockUser)
        {
            if (gameDockUser == null)
                throw new ArgumentNullException(nameof(gameDockUser));

            var entity = GameDockUserMapper.FromUserDto(gameDockUser);
            
            _context.GameDockUsers.Add(entity);
            _context.SaveChanges();
            
            return GameDockUserMapper.ToUserDto(entity);
        }

        public GameDockUserDto ReadUser(int id)
        {
            var entity = _context.GameDockUsers.Find(id);
            
            return GameDockUserMapper.ToUserDto(entity);
        }

        public void UpdateUser(GameDockUserDto gameDockUser)
        {
            if (gameDockUser == null)
                throw new ArgumentNullException(nameof(gameDockUser));

            var existingEntity = _context.GameDockUsers.Find(gameDockUser.GameDockUserId);
            
            if (existingEntity == null)
                throw new Exception($"No user found with ID {gameDockUser.GameDockUserId}");

            existingEntity.IsAdmin = gameDockUser.IsAdmin;
            existingEntity.UserName = gameDockUser.UserName;
            existingEntity.Email = gameDockUser.Email;
            existingEntity.PasswordHash = gameDockUser.PasswordHash;
            existingEntity.CreatedAt = gameDockUser.CreatedAt;

            _context.SaveChanges();
        }
        public void DeleteUser(int id)
        {
            var entity = _context.GameDockUsers.Find(id);
            if (entity == null)
                throw new Exception($"No user found with ID {id}");

            _context.GameDockUsers.Remove(entity);
            _context.SaveChanges();
        }

        public List<GameDockUserDto> GetAllUsers()
        {
            return _context.GameDockUsers.Select(GameDockUserMapper.ToUserDto).ToList();
        }
    }
}

