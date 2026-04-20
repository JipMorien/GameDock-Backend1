namespace Domain
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }

        public Post(int postId, string content, DateTime createdAt, int userId)
        {
            PostId = postId;
            Content = content;
            CreatedAt = createdAt;
            UserId = userId;
        }
    }
}
