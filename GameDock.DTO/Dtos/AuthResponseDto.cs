namespace GameDock.DTO.Dtos
{
    public class AuthResponseDto
    {
        public int GameDockUserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}

