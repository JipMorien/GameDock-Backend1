namespace Domain
{
    public class GameDockUser
    {
        public int GameDockUserId {get; set;}
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash {get; set;}
        public DateTime CreatedAt { get; set; }
    
    
        public GameDockUser(bool isAdmin, int gameDockUserId, string userName, string  email, string passwordHash, DateTime  createdAt)
        {
            IsAdmin = isAdmin;
            GameDockUserId = gameDockUserId;
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = createdAt;
        }
    }
}
