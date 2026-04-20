using Shared.Mappers;
using DTO.Dtos;
using DTO.Interfaces;

namespace DAL.Repos;

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

        existingEntity.UserId = profile.UserId;
        existingEntity.UserName = profile.UserName;

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
}