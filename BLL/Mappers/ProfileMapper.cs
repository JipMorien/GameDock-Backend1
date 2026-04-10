using DTO.Dtos;
using Domain;

namespace BLL.Mappers
{
    public static class ProfileMapper
    {
        public static Profile FromProfileDto(ProfileDto dto)
        {
            if (dto == null) 
                throw new ArgumentNullException(nameof(dto));

            return new Profile(
                dto.ProfileId,
                dto.UserName,
                dto.UserId);
        }
        
        public static Profile ToProfileDto(Profile profile)
        {
            if (profile == null) 
                throw new ArgumentNullException(nameof(profile));

            return new Profile(
                profile.ProfileId,
                profile.UserName,
                profile.UserId);
        }
        
        
        
        
    
    }
}