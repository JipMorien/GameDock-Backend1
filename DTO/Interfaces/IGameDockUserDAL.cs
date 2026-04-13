using DTO.Dtos;

namespace DTO.Interfaces
{
    public interface IGameDockUserDAL
    {
        GameDockUserDto CreateUser(GameDockUserDto gameDockUser);
        GameDockUserDto ReadUser(int id);
        void UpdateUser(GameDockUserDto gameDockUser);
        void DeleteUser(int id);
        List<GameDockUserDto> GetAllUsers();

    }
}