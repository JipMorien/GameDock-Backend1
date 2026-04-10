using Domain.Statistics;

namespace DTO.Dtos
{
    public class StatisticDto
    {
        public int StatisticId { get; set; }
        public int UserId { get; set; }
        public StatisticType StatisticType { get; set; }
        public float Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


