using DTO.Dtos;

namespace DTO.Interfaces
{
    public interface IStatisticDAL
    {
        StatisticDto CreateStatistic(StatisticDto statistic);
        StatisticDto ReadStatistic(int id);
        void UpdateStatistic(StatisticDto statistic);
        void DeleteStatistic(int id);
        List<StatisticDto> GetAllStatistics();
    }
    
}