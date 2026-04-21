using GameDock.BLL.Containers;
using GameDock.Shared.Mappers;
using GameDock.DTO.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GameDock.API.Controllers;

[ApiController]
[Route("api/leaderboards")]
public class LeaderboardsController : ControllerBase
{
    private readonly LeaderboardContainer _container;

    public LeaderboardsController(LeaderboardContainer container)
    {
        _container = container;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<LeaderboardDto>> GetAll()
    {
        var leaderboards = _container.GetAllLeaderboards()
            .Select(LeaderboardMapper.ToLeaderboardDto);

        return Ok(leaderboards);
    }

    [HttpGet("{id}")]
    public ActionResult<LeaderboardDto> GetById(int id)
    {
        var leaderboard = _container.ReadLeaderboard(id);

        if (leaderboard == null)
            return NotFound();

        return Ok(LeaderboardMapper.ToLeaderboardDto(leaderboard));
    }

    [HttpPost]
    public ActionResult<LeaderboardDto> Create([FromBody] LeaderboardDto leaderboardDto)
    {
        try
        {
            var leaderboard = LeaderboardMapper.FromLeaderboardDto(leaderboardDto);
            var created = _container.CreateLeaderboard(leaderboard);
            var createdDto = LeaderboardMapper.ToLeaderboardDto(created);

            return CreatedAtAction(nameof(GetById), new { id = createdDto.LeaderboardId }, createdDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] LeaderboardDto leaderboardDto)
    {
        if (id != leaderboardDto.LeaderboardId)
            return BadRequest();

        var existing = _container.ReadLeaderboard(id);
        if (existing == null)
            return NotFound();

        var leaderboard = LeaderboardMapper.FromLeaderboardDto(leaderboardDto);
        _container.UpdateLeaderboard(leaderboard);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _container.ReadLeaderboard(id);
        if (existing == null)
            return NotFound();

        _container.DeleteLeaderboard(id);
        return NoContent();
    }
}