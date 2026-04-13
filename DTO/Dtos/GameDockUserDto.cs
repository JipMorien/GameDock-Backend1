namespace DTO.Dtos
{
    public class GameDockUserDto
    {
        public int GameDockUserId {get; set;}
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash {get; set;}
        public DateTime CreatedAt { get; set; }
    }
}

