namespace GameDock.Domain
{
    public class Profile
    {
        public int ProfileId {get; set;}
        public string UserName { get; set; } = string.Empty;
        public int UserId {get; set;}
        public string Bio {get; set;} = string.Empty;
        public int AvatarId { get; set; } = 1;
        public DateTime CreatedAt {get; set;}
        


        public Profile(int profileId, string userName, int userId, string bio, int avatarId, DateTime createdAt)
        {
            ProfileId = profileId;
            UserName = userName;
            UserId = userId;
            Bio = bio;
            AvatarId = avatarId;
            CreatedAt = createdAt;
        }

        public Profile()
        {
            
        }
    }
    
}
