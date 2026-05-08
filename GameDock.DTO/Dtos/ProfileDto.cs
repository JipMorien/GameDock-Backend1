namespace GameDock.DTO.Dtos
{
    public class ProfileDto
    {
        public int ProfileId {get; set;}
        public string UserName { get; set; } = string.Empty;
        public int UserId {get; set;}
        public string Bio { get; set; } = string.Empty;
        public int AvatarId { get; set; } = 1;
        public DateTime CreatedAt { get; set; }
    }
}

