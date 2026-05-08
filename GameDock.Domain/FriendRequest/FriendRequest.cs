namespace GameDock.Domain.FriendRequest;

public class FriendRequest
{
    public int FriendRequestId { get; set; }

    public int SenderUserId { get; set; }

    public int ReceiverUserId { get; set; }

    public FriendRequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}