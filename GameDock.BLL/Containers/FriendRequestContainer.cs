using GameDock.Domain.FriendRequest;
using GameDock.DTO.Interfaces;
using GameDock.Shared.Mappers;

namespace GameDock.BLL.Containers;

public class FriendRequestContainer
{
    private readonly IFriendRequestDAL _friendRequestDAL;
    private readonly IGameDockUserDAL _userDAL;

    public FriendRequestContainer(IFriendRequestDAL friendRequestDAL, IGameDockUserDAL userDAL)
    {
        _friendRequestDAL = friendRequestDAL ?? throw new ArgumentNullException(nameof(friendRequestDAL));
        _userDAL = userDAL ?? throw new ArgumentNullException(nameof(userDAL));

    }
    

    private void CheckId(int id, string name)
    {
        if (id <= 0)
            throw new ArgumentException($"{name} must be greater than 0");
    }

    public void CheckFriendRequest(FriendRequest friendRequest)
    {
        if (friendRequest == null)
            throw new ArgumentNullException(nameof(friendRequest));

        if (friendRequest.FriendRequestId < 0)
            throw new ArgumentException("Friend request ID cannot be less than 0");

        CheckId(friendRequest.SenderUserId, "Sender user ID");
        CheckId(friendRequest.ReceiverUserId, "Receiver user ID");

        if (friendRequest.SenderUserId == friendRequest.ReceiverUserId)
            throw new ArgumentException("You cannot send a friend request to yourself");

        if (friendRequest.CreatedAt == default)
            throw new ArgumentException("CreatedAt cannot be empty");
    }

    private FriendRequest GetExistingFriendRequest(int requestId)
    {
        CheckId(requestId, "Friend request ID");

        var dto = _friendRequestDAL.ReadFriendRequest(requestId);

        if (dto == null)
            throw new ArgumentException("Friend request could not be read");

        return FriendRequestMapper.FromFriendRequestDto(dto);
    }

    private void CheckReceiverAccess(FriendRequest friendRequest, int currentUserId)
    {
        CheckId(currentUserId, "Current user ID");

        if (friendRequest.ReceiverUserId != currentUserId)
            throw new UnauthorizedAccessException("You can only manage friend requests sent to you");
    }

    private void CheckRequestIsPending(FriendRequest friendRequest)
    {
        if (friendRequest.Status != FriendRequestStatus.Pending)
            throw new ArgumentException("Only pending friend requests can be changed");
    }

    public FriendRequest CreateFriendRequest(int senderUserId, int receiverUserId)
    {
        CheckId(senderUserId, "Sender user ID");
        CheckId(receiverUserId, "Receiver user ID");

        if (senderUserId == receiverUserId)
            throw new ArgumentException("You cannot send a friend request to yourself");

        var existingRequest = _friendRequestDAL.GetFriendRequestBetweenUsers(senderUserId, receiverUserId);

        if (existingRequest != null)
            throw new ArgumentException("A friend request between these users already exists");

        var friendRequest = new FriendRequest
        {
            SenderUserId = senderUserId,
            ReceiverUserId = receiverUserId,
            Status = FriendRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        CheckFriendRequest(friendRequest);

        var createdDto = _friendRequestDAL.CreateFriendRequest(
            FriendRequestMapper.ToFriendRequestDto(friendRequest)
        );

        return FriendRequestMapper.FromFriendRequestDto(createdDto);
    }

    public FriendRequest ReadFriendRequest(int id)
    {
        return GetExistingFriendRequest(id);
    }

    public void AcceptFriendRequest(int requestId, int currentUserId)
    {
        var friendRequest = GetExistingFriendRequest(requestId);

        CheckReceiverAccess(friendRequest, currentUserId);
        CheckRequestIsPending(friendRequest);

        friendRequest.Status = FriendRequestStatus.Accepted;

        _friendRequestDAL.UpdateFriendRequest(
            FriendRequestMapper.ToFriendRequestDto(friendRequest)
        );
    }

    public void RejectFriendRequest(int requestId, int currentUserId)
    {
        var friendRequest = GetExistingFriendRequest(requestId);

        CheckReceiverAccess(friendRequest, currentUserId);
        CheckRequestIsPending(friendRequest);

        friendRequest.Status = FriendRequestStatus.Rejected;

        _friendRequestDAL.UpdateFriendRequest(
            FriendRequestMapper.ToFriendRequestDto(friendRequest)
        );
    }

    public void DeleteFriendRequest(int requestId, int currentUserId)
    {
        var friendRequest = GetExistingFriendRequest(requestId);

        CheckId(currentUserId, "Current user ID");

        if (friendRequest.SenderUserId != currentUserId &&
            friendRequest.ReceiverUserId != currentUserId)
            throw new UnauthorizedAccessException("You can only delete your own friend requests");

        _friendRequestDAL.DeleteFriendRequest(requestId);
    }

    public List<FriendRequest> GetAllFriendRequests()
    {
        return _friendRequestDAL.GetAllFriendRequests()
            .Select(FriendRequestMapper.FromFriendRequestDto)
            .ToList();
    }

    public List<FriendRequest> GetIncomingRequests(int receiverUserId)
    {
        CheckId(receiverUserId, "Receiver user ID");

        return _friendRequestDAL.GetFriendRequestsByReceiverId(receiverUserId)
            .Select(FriendRequestMapper.FromFriendRequestDto)
            .Where(request => request.Status == FriendRequestStatus.Pending)
            .ToList();
    }

    public List<FriendRequest> GetOutgoingRequests(int senderUserId)
    {
        CheckId(senderUserId, "Sender user ID");

        return _friendRequestDAL.GetFriendRequestsBySenderId(senderUserId)
            .Select(FriendRequestMapper.FromFriendRequestDto)
            .Where(request => request.Status == FriendRequestStatus.Pending)
            .ToList();
    }

    public List<FriendRequest> GetFriends(int userId)
    {
        CheckId(userId, "User ID");

        return _friendRequestDAL.GetAcceptedFriends(userId)
            .Select(FriendRequestMapper.FromFriendRequestDto)
            .ToList();
    }
    
    public FriendRequest CreateFriendRequestByUserName(int senderUserId, string receiverUserName)
    {
        CheckId(senderUserId, "Sender user ID");

        if (string.IsNullOrWhiteSpace(receiverUserName))
            throw new ArgumentException("Username cannot be empty");

        var receiverUser = _userDAL.GetUserByUserName(receiverUserName);

        if (receiverUser == null)
            throw new ArgumentException("User with this username does not exist");

        return CreateFriendRequest(senderUserId, receiverUser.GameDockUserId);
    }
}