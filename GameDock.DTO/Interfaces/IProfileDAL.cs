using DTO.Dtos;

namespace DTO.Interfaces
{
    public interface IProfileDAL
    {
        ProfileDto CreateProfile(ProfileDto profile);
        ProfileDto? ReadProfile(int id);
        void UpdateProfile(ProfileDto profile);
        void DeleteProfile(int id);
        List<ProfileDto> GetAllProfiles();

    }
}