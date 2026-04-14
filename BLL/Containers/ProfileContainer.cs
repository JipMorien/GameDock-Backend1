using DTO.Interfaces;
using Domain;
using Shared.Mappers;

namespace BLL.Containers
{
    public class ProfileContainer
    {
        private readonly IProfileDAL _profileDAL;

        public ProfileContainer(IProfileDAL profileDAL)
        {
            _profileDAL = profileDAL ?? throw new ArgumentNullException(nameof(profileDAL));
        }

        public void CheckProfile(Profile profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (profile.ProfileId < 0)
                throw new ArgumentException("Profile ID cannot be less than 0");

            if (string.IsNullOrWhiteSpace(profile.UserName))
                throw new ArgumentException("Username cannot be empty");

            if (profile.UserId < 0)
                throw new ArgumentException("User ID cannot be less than 0");
        }

        public Profile CreateProfile(Profile profile)
        {
            CheckProfile(profile);

            var newProfileDto = _profileDAL.CreateProfile(ProfileMapper.ToProfileDto(profile));

            if (newProfileDto == null)
                throw new Exception("Profile could not be created");

            return ProfileMapper.FromProfileDto(newProfileDto);
        }

        public Profile ReadProfile(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be 0 or negative");

            var profileDto = _profileDAL.ReadProfile(id);

            if (profileDto == null)
                throw new ArgumentException("Profile could not be read");

            return ProfileMapper.FromProfileDto(profileDto);
        }

        public void UpdateProfile(Profile profile)
        {
            CheckProfile(profile);

            var existingProfile = _profileDAL.ReadProfile(profile.ProfileId);
            if (existingProfile == null)
                throw new ArgumentException("Profile could not be read");

            _profileDAL.UpdateProfile(ProfileMapper.ToProfileDto(profile));
        }

        public void DeleteProfile(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be 0 or negative");

            var existingProfile = _profileDAL.ReadProfile(id);
            if (existingProfile == null)
                throw new ArgumentException("Profile could not be read");

            _profileDAL.DeleteProfile(id);
        }

        public List<Profile> GetAllProfiles()
        {
            var profileDtos = _profileDAL.GetAllProfiles();

            return profileDtos.Select(ProfileMapper.FromProfileDto).ToList();
        }
    }
}