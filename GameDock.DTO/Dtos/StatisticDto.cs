using GameDock.DTO.Dtos.Enums;

namespace GameDock.DTO.Dtos
{
    public class StatisticDto
    {
        public int StatisticId { get; set; }
        public int UserId { get; set; }
        public StatisticTypeDto StatisticType { get; set; }
        public float Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


