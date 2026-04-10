using DTO.Dtos;
using Domain;

namespace BLL.Mappers
{
    public static class UserMapper
    {
        public static User FromUserDto(UserDto dto)
        {
            if (dto == null) 
                throw new ArgumentNullException(nameof(dto));


            return new User(
                dto.IsAdmin,
                dto.UserId,
                dto.UserName,
                dto.Email,
                dto.PasswordHash,
                dto.CreatedAt);
        }
        
        public static UserDto ToUserDto(User user)
        {
            if (user == null) 
                throw new ArgumentNullException(nameof(user));

            return new UserDto
            {
                IsAdmin = user.IsAdmin,
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                CreatedAt = user.CreatedAt
            };
            
        }
        
        
        
        
    
    }
}