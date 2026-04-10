namespace DTO.Dtos
{
    public class UserDto
    {
        public bool IsAdmin { get; set; }
        public int UserId {get; set;}
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash {get; set;}
        public DateTime CreatedAt { get; set; }
    }
}

