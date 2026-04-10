namespace Domain.Statistics
{
    public class Statistic
    {
        public int StatisticId { get; set; }
        public int UserId { get; set; }
        public StatisticType StatisticType { get; set; }
        public float Value { get; set; }
        public DateTime CreatedAt { get; set; }

        public Statistic(int statisticId, int userId, StatisticType statisticType, float value, DateTime createdAt)
        {
            StatisticId = statisticId;
            UserId = userId;
            StatisticType = statisticType;
            Value = value;
            CreatedAt = createdAt;
        }
    }
}

