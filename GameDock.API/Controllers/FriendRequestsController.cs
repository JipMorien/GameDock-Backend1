using System.Security.Claims;
using GameDock.BLL.Containers;
using GameDock.DTO.Dtos;
using GameDock.Shared.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameDock.API.Controllers;

[Authorize]
[ApiController]
[Route("api/friends")]
public class FriendRequestsController : ControllerBase
{
    private readonly FriendRequestContainer _container;

    public FriendRequestsController(FriendRequestContainer container)
    {
        _container = container;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID claim not found");

        if (!int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("Invalid user ID claim");

        return userId;
    }

    [HttpPost("request/{receiverUserId:int}")]
    public ActionResult<FriendRequestDto> SendFriendRequest(int receiverUserId)
    {
        try
        {
            var senderUserId = GetCurrentUserId();

            var created = _container.CreateFriendRequest(senderUserId, receiverUserId);

            return Ok(FriendRequestMapper.ToFriendRequestDto(created));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("incoming")]
    public ActionResult<IEnumerable<FriendRequestDto>> GetIncomingRequests()
    {
        var userId = GetCurrentUserId();

        var requests = _container.GetIncomingRequests(userId)
            .Select(FriendRequestMapper.ToFriendRequestDto);

        return Ok(requests);
    }

    [HttpGet("outgoing")]
    public ActionResult<IEnumerable<FriendRequestDto>> GetOutgoingRequests()
    {
        var userId = GetCurrentUserId();

        var requests = _container.GetOutgoingRequests(userId)
            .Select(FriendRequestMapper.ToFriendRequestDto);

        return Ok(requests);
    }

    [HttpGet]
    public ActionResult<IEnumerable<FriendRequestDto>> GetFriends()
    {
        var userId = GetCurrentUserId();

        var friends = _container.GetFriends(userId)
            .Select(FriendRequestMapper.ToFriendRequestDto);

        return Ok(friends);
    }

    [HttpPut("accept/{requestId:int}")]
    public IActionResult AcceptFriendRequest(int requestId)
    {
        try
        {
            var userId = GetCurrentUserId();

            _container.AcceptFriendRequest(requestId, userId);

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("reject/{requestId:int}")]
    public IActionResult RejectFriendRequest(int requestId)
    {
        try
        {
            var userId = GetCurrentUserId();

            _container.RejectFriendRequest(requestId, userId);

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{requestId:int}")]
    public IActionResult DeleteFriendRequest(int requestId)
    {
        try
        {
            var userId = GetCurrentUserId();

            _container.DeleteFriendRequest(requestId, userId);

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}