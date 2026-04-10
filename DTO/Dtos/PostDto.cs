namespace DTO.Dtos
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}

