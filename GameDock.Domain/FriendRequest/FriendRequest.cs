using System.ComponentModel.DataAnnotations.Schema;

namespace GameDock.Domain.FriendRequest;

public class FriendRequest
{
    public int FriendRequestId { get; set; }

    public int SenderUserId { get; set; }
    [NotMapped]
    public string SenderUserName { get; set; } = string.Empty;

    public int ReceiverUserId { get; set; }
    [NotMapped]
    public string ReceiverUserName { get; set; } = string.Empty;

    public FriendRequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}