using DTO.Dtos;

namespace DTO.Interfaces
{
    public interface IUserDAL
    {
        UserDto CreateUser(UserDto user);
        UserDto ReadUser(int id);
        void UpdateUser(UserDto user);
        void DeleteUser(int id);
        List<UserDto> GetAllUsers();

    }
}