using Shared.Mappers;
using DTO.Dtos;
using DTO.Interfaces;

namespace DAL.Repos
{
    public class StatisticDAL : IStatisticDAL
    {
        private readonly AppDbContext _context;

        public StatisticDAL(AppDbContext context)
        {
            _context = context;
        }

        public StatisticDto CreateStatistic(StatisticDto statistic)
        {
            if (statistic == null)
                throw new ArgumentNullException(nameof(statistic));

            var entity = StatisticMapper.FromStatisticDto(statistic);

            _context.Statistics.Add(entity);
            _context.SaveChanges();

            return StatisticMapper.ToStatisticDto(entity);
        }

        public StatisticDto? ReadStatistic(int id)
        {
            var entity = _context.Statistics.Find(id);

            if (entity == null)
                return null;

            return StatisticMapper.ToStatisticDto(entity);
        }

        public void UpdateStatistic(StatisticDto statistic)
        {
            if (statistic == null)
                throw new ArgumentNullException(nameof(statistic));

            var existingEntity = _context.Statistics.Find(statistic.StatisticId);

            if (existingEntity == null)
                throw new Exception($"No statistic found with ID {statistic.StatisticId}");

            existingEntity.UserId = statistic.UserId;
            existingEntity.StatisticType = (Domain.Statistics.StatisticType)statistic.StatisticType;
            existingEntity.Value = statistic.Value;
            existingEntity.CreatedAt = statistic.CreatedAt;

            _context.SaveChanges();
        }

        public void DeleteStatistic(int id)
        {
            var entity = _context.Statistics.Find(id);

            if (entity == null)
                throw new Exception($"No statistic found with ID {id}");

            _context.Statistics.Remove(entity);
            _context.SaveChanges();
        }

        public List<StatisticDto> GetAllStatistics()
        {
            return _context.Statistics.Select(StatisticMapper.ToStatisticDto).ToList();
        }
    }
}