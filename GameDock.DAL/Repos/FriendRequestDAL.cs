using GameDock.Domain.FriendRequest;
using GameDock.DTO.Dtos;
using GameDock.DTO.Interfaces;
using GameDock.Shared.Mappers;
using Microsoft.EntityFrameworkCore;

namespace GameDock.DAL.Repos;

public class FriendRequestDAL : IFriendRequestDAL
{
    private readonly AppDbContext _context;

    public FriendRequestDAL(AppDbContext context)
    {
        _context = context;
    }

    public FriendRequestDto CreateFriendRequest(FriendRequestDto friendRequest)
    {
        if (friendRequest == null)
            throw new ArgumentNullException(nameof(friendRequest));

        var entity = FriendRequestMapper.FromFriendRequestDto(friendRequest);

        _context.FriendRequests.Add(entity);
        _context.SaveChanges();

        return ToDtoWithUserNames(entity);
    }

    public FriendRequestDto? ReadFriendRequest(int id)
    {
        var entity = _context.FriendRequests
            .AsNoTracking()
            .FirstOrDefault(request => request.FriendRequestId == id);

        return entity == null ? null : ToDtoWithUserNames(entity);
    }

    public void UpdateFriendRequest(FriendRequestDto friendRequest)
    {
        if (friendRequest == null)
            throw new ArgumentNullException(nameof(friendRequest));

        var existingEntity = _context.FriendRequests.Find(friendRequest.FriendRequestId);

        if (existingEntity == null)
            throw new Exception($"Friend request not found with ID {friendRequest.FriendRequestId}");

        existingEntity.SenderUserId = friendRequest.SenderUserId;
        existingEntity.ReceiverUserId = friendRequest.ReceiverUserId;
        existingEntity.Status = (FriendRequestStatus)friendRequest.Status;

        _context.SaveChanges();
    }

    public void DeleteFriendRequest(int id)
    {
        var entity = _context.FriendRequests.Find(id);

        if (entity == null)
            throw new Exception($"Friend request not found with ID {id}");

        _context.FriendRequests.Remove(entity);
        _context.SaveChanges();
    }

    public List<FriendRequestDto> GetAllFriendRequests()
    {
        return _context.FriendRequests
            .AsNoTracking()
            .ToList()
            .Select(ToDtoWithUserNames)
            .ToList();
    }

    public List<FriendRequestDto> GetFriendRequestsByReceiverId(int receiverUserId)
    {
        return _context.FriendRequests
            .AsNoTracking()
            .Where(request => request.ReceiverUserId == receiverUserId)
            .ToList()
            .Select(ToDtoWithUserNames)
            .ToList();
    }

    public List<FriendRequestDto> GetFriendRequestsBySenderId(int senderUserId)
    {
        return _context.FriendRequests
            .AsNoTracking()
            .Where(request => request.SenderUserId == senderUserId)
            .ToList()
            .Select(ToDtoWithUserNames)
            .ToList();
    }

    public List<FriendRequestDto> GetAcceptedFriends(int userId)
    {
        return _context.FriendRequests
            .AsNoTracking()
            .Where(request =>
                request.Status == FriendRequestStatus.Accepted &&
                (request.SenderUserId == userId || request.ReceiverUserId == userId))
            .ToList()
            .Select(ToDtoWithUserNames)
            .ToList();
    }

    public FriendRequestDto? GetFriendRequestBetweenUsers(int senderUserId, int receiverUserId)
    {
        var entity = _context.FriendRequests
            .AsNoTracking()
            .FirstOrDefault(request =>
                (request.SenderUserId == senderUserId && request.ReceiverUserId == receiverUserId) ||
                (request.SenderUserId == receiverUserId && request.ReceiverUserId == senderUserId));

        return entity == null ? null : ToDtoWithUserNames(entity);
    }

    private FriendRequestDto ToDtoWithUserNames(FriendRequest request)
    {
        var senderUserName = _context.GameDockUsers
            .AsNoTracking()
            .Where(user => user.GameDockUserId == request.SenderUserId)
            .Select(user => user.UserName)
            .FirstOrDefault() ?? string.Empty;

        var receiverUserName = _context.GameDockUsers
            .AsNoTracking()
            .Where(user => user.GameDockUserId == request.ReceiverUserId)
            .Select(user => user.UserName)
            .FirstOrDefault() ?? string.Empty;

        return FriendRequestMapper.ToFriendRequestDto(
            request,
            senderUserName,
            receiverUserName
        );
    }
}