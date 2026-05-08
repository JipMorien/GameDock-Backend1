using GameDock.DTO.Dtos.Enums;

namespace GameDock.DTO.Dtos;

public class FriendRequestDto
{
    public int FriendRequestId { get; set; }

    public int SenderUserId { get; set; }

    public int ReceiverUserId { get; set; }

    public FriendRequestStatusDto Status { get; set; }

    public DateTime CreatedAt { get; set; }
}