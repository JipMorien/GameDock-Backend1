using GameDock.DTO.Dtos.Enums;

namespace GameDock.DTO.Dtos;

public class FriendRequestDto
{
    public int FriendRequestId { get; set; }

    public int SenderUserId { get; set; }
    public string SenderUserName { get; set; } = string.Empty;

    public int ReceiverUserId { get; set; }
    public string ReceiverUserName { get; set; } = string.Empty;

    public FriendRequestStatusDto Status { get; set; }

    public DateTime CreatedAt { get; set; }
}