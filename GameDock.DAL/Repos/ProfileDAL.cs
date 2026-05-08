using GameDock.DTO.Dtos;
using GameDock.DTO.Interfaces;
using GameDock.Shared.Mappers;

namespace GameDock.DAL.Repos;

public class ProfileDAL : IProfileDAL
{
    private readonly AppDbContext _context;

    public ProfileDAL(AppDbContext context)
    {
        _context = context;
    }

    public ProfileDto CreateProfile(ProfileDto profile)
    {
        if  (profile == null)
            throw new ArgumentNullException(nameof(profile));
        
        var entity = ProfileMapper.FromProfileDto(profile);
        
        _context.Profiles.Add(entity);
        _context.SaveChanges();
        
        return ProfileMapper.ToProfileDto(entity);
    }

    public ProfileDto? ReadProfile(int id)
    {
        var entity = _context.Profiles.Find(id);
        
        if (entity == null)
            return null;
        
        return ProfileMapper.ToProfileDto(entity);
    }

    public void UpdateProfile(ProfileDto profile)
    {
        if (profile == null)
            throw new ArgumentNullException(nameof(profile));

        var existingEntity = _context.Profiles.Find(profile.ProfileId);

        if (existingEntity == null)
            throw new Exception($"Profile not found with ID {profile.ProfileId}");

        existingEntity.UserName = profile.UserName;
        existingEntity.Bio = profile.Bio;
        existingEntity.AvatarId = profile.AvatarId;

        _context.SaveChanges();
    }

    public void DeleteProfile(int id)
    {
        var entity = _context.Profiles.Find(id);
        if (entity == null)
            throw new Exception($"Profile not found with ID {id}");
        
        _context.Profiles.Remove(entity);
        _context.SaveChanges();
    }

    public List<ProfileDto> GetAllProfiles()
    {
        return _context.Profiles.Select(ProfileMapper.ToProfileDto).ToList();
    }

    public ProfileDto? GetProfileByUserId(int userId)
    {
        var entity = _context.Profiles
            .FirstOrDefault(profile => profile.UserId == userId);

        if (entity == null)
            return null;

        return ProfileMapper.ToProfileDto(entity);
    }
}