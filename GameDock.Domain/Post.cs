namespace GameDock.Domain
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }

        public Post(int postId, string title, string content, DateTime createdAt, int userId)
        {
            PostId = postId;
            Title = title;
            Content = content;
            CreatedAt = createdAt;
            UserId = userId;
        }
    }
}
