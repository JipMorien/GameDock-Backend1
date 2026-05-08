using GameDock.BLL.Containers;
using GameDock.Shared.Mappers;
using GameDock.DTO.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GameDock.API.Controllers;

[Authorize]
[ApiController]
[Route("api/profiles")]
public class ProfilesController : ControllerBase
{
    private readonly ProfileContainer _container;

    public ProfilesController(ProfileContainer container)
    {
        _container = container;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProfileDto>> GetAll()
    {
        var profiles = _container.GetAllProfiles()
            .Select(ProfileMapper.ToProfileDto);

        return Ok(profiles);
    }

    [HttpGet("{id}")]
    public ActionResult<ProfileDto> GetById(int id)
    {
        var profile = _container.ReadProfile(id);

        if (profile == null)
            return NotFound();

        return Ok(ProfileMapper.ToProfileDto(profile));
    }

    [HttpPost]
    public ActionResult<ProfileDto> Create([FromBody] ProfileDto profileDto)
    {
        try
        {
            var profile = ProfileMapper.FromProfileDto(profileDto);
            var created = _container.CreateProfile(profile);
            var createdDto = ProfileMapper.ToProfileDto(created);

            return CreatedAtAction(nameof(GetById), new { id = createdDto.ProfileId }, createdDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProfileDto profileDto)
    {
        if (id != profileDto.ProfileId)
            return BadRequest();

        var existing = _container.ReadProfile(id);
        if (existing == null)
            return NotFound();

        var profile = ProfileMapper.FromProfileDto(profileDto);
        _container.UpdateProfile(profile);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _container.ReadProfile(id);
        if (existing == null)
            return NotFound();

        _container.DeleteProfile(id);
        return NoContent();
    }
}