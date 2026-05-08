using GameDock.BLL.Containers;
using GameDock.Shared.Mappers;
using GameDock.DTO.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GameDock.API.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly PostContainer _container;

    public PostsController(PostContainer container)
    {
        _container = container;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetAll()
    {
        var posts = _container.GetAllPosts()
            .Select(PostMapper.ToPostDto);

        return Ok(posts);
    }

    [HttpGet("{id}")]
    public ActionResult<PostDto> GetById(int id)
    {
        var post = _container.ReadPost(id);

        if (post == null)
            return NotFound();

        return Ok(PostMapper.ToPostDto(post));
    }

    [Authorize]
    [HttpPost]
    public ActionResult<PostDto> Create([FromBody] PostDto postDto)
    {
        try
        {
            var post = PostMapper.FromPostDto(postDto);
            var created = _container.CreatePost(post);
            var createdDto = PostMapper.ToPostDto(created);

            return CreatedAtAction(nameof(GetById), new { id = createdDto.PostId }, createdDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [Authorize]
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] PostDto postDto)
    {
        if (id != postDto.PostId)
            return BadRequest();

        var existing = _container.ReadPost(id);
        if (existing == null)
            return NotFound();

        var post = PostMapper.FromPostDto(postDto);
        _container.UpdatePost(post);

        return NoContent();
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _container.ReadPost(id);
        if (existing == null)
            return NotFound();

        _container.DeletePost(id);
        return NoContent();
    }
}