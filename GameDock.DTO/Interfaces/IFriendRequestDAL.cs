using GameDock.DTO.Dtos;

namespace GameDock.DTO.Interfaces;

public interface IFriendRequestDAL
{
    FriendRequestDto CreateFriendRequest(FriendRequestDto friendRequest);

    FriendRequestDto? ReadFriendRequest(int id);

    void UpdateFriendRequest(FriendRequestDto friendRequest);

    void DeleteFriendRequest(int id);

    List<FriendRequestDto> GetAllFriendRequests();

    List<FriendRequestDto> GetFriendRequestsByReceiverId(int receiverUserId);

    List<FriendRequestDto> GetFriendRequestsBySenderId(int senderUserId);

    List<FriendRequestDto> GetAcceptedFriends(int userId);

    FriendRequestDto? GetFriendRequestBetweenUsers(int senderUserId, int receiverUserId);
}