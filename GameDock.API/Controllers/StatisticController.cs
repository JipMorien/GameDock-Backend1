using GameDock.BLL.Containers;
using GameDock.Shared.Mappers;
using GameDock.DTO.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GameDock.API.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly StatisticContainer _container;

    public StatisticsController(StatisticContainer container)
    {
        _container = container;
    }

    [HttpGet]
    public ActionResult<IEnumerable<StatisticDto>> GetAll()
    {
        var statistics = _container.GetAllStatistics()
            .Select(StatisticMapper.ToStatisticDto);

        return Ok(statistics);
    }

    [HttpGet("{id}")]
    public ActionResult<StatisticDto> GetById(int id)
    {
        var statistic = _container.ReadStatistic(id);

        if (statistic == null)
            return NotFound();

        return Ok(StatisticMapper.ToStatisticDto(statistic));
    }

    [HttpPost]
    public ActionResult<StatisticDto> Create([FromBody] StatisticDto statisticDto)
    {
        try
        {
            var statistic = StatisticMapper.FromStatisticDto(statisticDto);
            var created = _container.CreateStatistic(statistic);
            var createdDto = StatisticMapper.ToStatisticDto(created);

            return CreatedAtAction(nameof(GetById), new { id = createdDto.StatisticId }, createdDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] StatisticDto statisticDto)
    {
        if (id != statisticDto.StatisticId)
            return BadRequest();

        var existing = _container.ReadStatistic(id);
        if (existing == null)
            return NotFound();

        var statistic = StatisticMapper.FromStatisticDto(statisticDto);
        _container.UpdateStatistic(statistic);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _container.ReadStatistic(id);
        if (existing == null)
            return NotFound();

        _container.DeleteStatistic(id);
        return NoContent();
    }
}