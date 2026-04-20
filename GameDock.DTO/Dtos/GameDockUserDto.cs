namespace DTO.Dtos
{
    public class GameDockUserDto
    {
        public int GameDockUserId {get; set;}
        public bool IsAdmin { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash {get; set;}
        public DateTime CreatedAt { get; set; }
    }
}

