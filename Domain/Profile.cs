namespace Domain
{
    public class Profile
    {
        public int ProfileId {get; set;}
        public string UserName {get; set;}
        public int UserId {get; set;}


        public Profile(int profileId, string userName, int userId)
        {
            profileId = ProfileId;
            userName = UserName;
            userId = UserId;
        }
    }
    
}
