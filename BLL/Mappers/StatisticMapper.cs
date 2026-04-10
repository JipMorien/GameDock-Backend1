using DTO.Dtos;
using Domain.Statistics;

namespace BLL.Mappers
{
    public static class StatisticMapper
    {
        public static Statistic FromStatisticDto(StatisticDto dto)
        {
            if (dto == null) 
                throw new ArgumentNullException(nameof(dto));


            return new Statistic(
                dto.StatisticId,
                dto.UserId,
                dto.StatisticType,
                dto.Value,
                dto.CreatedAt
                );
        }
        
        public static StatisticDto ToStatisticDto(Statistic statistic)
        {
            if (statistic == null) 
                throw new ArgumentNullException(nameof(statistic));

            return new StatisticDto
            {
                StatisticId = statistic.StatisticId,
                UserId = statistic.UserId,
                StatisticType = statistic.StatisticType,
                Value = statistic.Value,
                CreatedAt = statistic.CreatedAt
            };
            
        }
        
        
        
        
    
    }
}