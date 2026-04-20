using Domain;
using DTO.Dtos;

namespace Shared.Mappers
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
        
        public static ProfileDto ToProfileDto(Profile profile)
        {
            if (profile == null) 
                throw new ArgumentNullException(nameof(profile));

            return new ProfileDto
            {
                ProfileId = profile.ProfileId,
                UserName = profile.UserName,
                UserId = profile.UserId
            };

        }
        
        
        
        
    
    }
}