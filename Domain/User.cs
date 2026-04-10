namespace Domain
{
    public class User
    {
        public bool IsAdmin { get; set; }
        public int UserId {get; set;}
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash {get; set;}
        public DateTime CreatedAt { get; set; }
    
    
        public User(bool isAdmin, int userId, string userName, string  email, string passwordHash, DateTime  createdAt)
        {
            isAdmin = IsAdmin;
            userId = UserId;
            userName = UserName;
            email = Email;
            passwordHash = PasswordHash;
            createdAt = CreatedAt;
        }
    }
}
