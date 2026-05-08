using GameDock.BLL.Containers;
using GameDock.Shared.Mappers;
using GameDock.DTO.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GameDock.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class GameDockUsersController : ControllerBase
{
    private readonly GameDockUserContainer _container;

    public GameDockUsersController(GameDockUserContainer container)
    {
        _container = container;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GameDockUserDto>> GetAll()
    {
        var users = _container.GetAllUsers()
            .Select(GameDockUserMapper.ToUserDto);

        return Ok(users);
    }

    [HttpGet("{id}")]
    public ActionResult<GameDockUserDto> GetById(int id)
    {
        var user = _container.ReadUser(id);

        if (user == null)
            return NotFound();

        return Ok(GameDockUserMapper.ToUserDto(user));
    }

    [HttpPost]
    public ActionResult<GameDockUserDto> Create([FromBody] GameDockUserDto gameDockUserDto)
    {
        try
        {
            var user = GameDockUserMapper.FromUserDto(gameDockUserDto);
            var created = _container.CreateUser(user);
            var createdDto = GameDockUserMapper.ToUserDto(created);

            return CreatedAtAction(nameof(GetById), new { id = createdDto.GameDockUserId }, createdDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] GameDockUserDto gameDockUserDto)
    {
        if (id != gameDockUserDto.GameDockUserId)
            return BadRequest();

        var existing = _container.ReadUser(id);
        if (existing == null)
            return NotFound();

        var user = GameDockUserMapper.FromUserDto(gameDockUserDto);
        _container.UpdateUser(user);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _container.ReadUser(id);
        if (existing == null)
            return NotFound();

        _container.DeleteUser(id);
        return NoContent();
    }
}