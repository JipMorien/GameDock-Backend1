using GameDock.Domain.FriendRequest;
using GameDock.DTO.Dtos;
using GameDock.DTO.Dtos.Enums;

namespace GameDock.Shared.Mappers;

public static class FriendRequestMapper
{
    public static FriendRequestDto ToFriendRequestDto(FriendRequest friendRequest)
    {
        return new FriendRequestDto
        {
            FriendRequestId = friendRequest.FriendRequestId,
            SenderUserId = friendRequest.SenderUserId,
            ReceiverUserId = friendRequest.ReceiverUserId,
            Status = (FriendRequestStatusDto)friendRequest.Status,
            CreatedAt = friendRequest.CreatedAt
        };
    }

    public static FriendRequestDto ToFriendRequestDto(
        FriendRequest friendRequest,
        string senderUserName,
        string receiverUserName)
    {
        return new FriendRequestDto
        {
            FriendRequestId = friendRequest.FriendRequestId,

            SenderUserId = friendRequest.SenderUserId,
            SenderUserName = senderUserName,

            ReceiverUserId = friendRequest.ReceiverUserId,
            ReceiverUserName = receiverUserName,

            Status = (FriendRequestStatusDto)friendRequest.Status,
            CreatedAt = friendRequest.CreatedAt
        };
    }

    public static FriendRequest FromFriendRequestDto(FriendRequestDto friendRequestDto)
    {
        return new FriendRequest
        {
            FriendRequestId = friendRequestDto.FriendRequestId,

            SenderUserId = friendRequestDto.SenderUserId,
            SenderUserName = friendRequestDto.SenderUserName,

            ReceiverUserId = friendRequestDto.ReceiverUserId,
            ReceiverUserName = friendRequestDto.ReceiverUserName,

            Status = (FriendRequestStatus)friendRequestDto.Status,
            CreatedAt = friendRequestDto.CreatedAt
        };
    }
}