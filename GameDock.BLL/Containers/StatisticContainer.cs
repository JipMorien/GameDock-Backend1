using DTO.Interfaces;
using Domain.Statistics;
using Shared.Mappers;

namespace BLL.Containers
{
    public class StatisticContainer
    {
        private readonly IStatisticDAL _statisticDAL;

        public StatisticContainer(IStatisticDAL statisticDAL)
        {
            _statisticDAL = statisticDAL ?? throw new ArgumentNullException(nameof(statisticDAL));
        }

        public void CheckStatistic(Statistic statistic)
        {
            if (statistic == null)
                throw new ArgumentNullException(nameof(statistic));

            if (statistic.StatisticId < 0)
                throw new ArgumentException("Statistic ID cannot be less than 0");

            if (statistic.UserId < 0)
                throw new ArgumentException("User ID cannot be less than 0");

            if (statistic.Value < 0)
                throw new ArgumentException("Statistic value cannot be less than 0");

            if (statistic.CreatedAt == default)
                throw new ArgumentException("CreatedAt must be valid");
        }

        public Statistic CreateStatistic(Statistic statistic)
        {
            CheckStatistic(statistic);

            var newStatisticDto = _statisticDAL.CreateStatistic(StatisticMapper.ToStatisticDto(statistic));

            if (newStatisticDto == null)
                throw new Exception("Statistic could not be created");

            return StatisticMapper.FromStatisticDto(newStatisticDto);
        }

        public Statistic ReadStatistic(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be 0 or negative");

            var statisticDto = _statisticDAL.ReadStatistic(id);

            if (statisticDto == null)
                throw new ArgumentException("Statistic could not be read");

            return StatisticMapper.FromStatisticDto(statisticDto);
        }

        public void UpdateStatistic(Statistic statistic)
        {
            CheckStatistic(statistic);

            var existingStatistic = _statisticDAL.ReadStatistic(statistic.StatisticId);
            if (existingStatistic == null)
                throw new ArgumentException("Statistic could not be read");

            _statisticDAL.UpdateStatistic(StatisticMapper.ToStatisticDto(statistic));
        }

        public void DeleteStatistic(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be 0 or negative");

            var existingStatistic = _statisticDAL.ReadStatistic(id);
            if (existingStatistic == null)
                throw new ArgumentException("Statistic could not be read");

            _statisticDAL.DeleteStatistic(id);
        }

        public List<Statistic> GetAllStatistics()
        {
            var statisticDtos = _statisticDAL.GetAllStatistics();

            return statisticDtos.Select(StatisticMapper.FromStatisticDto).ToList();
        }
    }
}