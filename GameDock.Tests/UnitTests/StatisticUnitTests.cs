using GameDock.BLL.Containers;
using GameDock.Domain.Statistics;
using GameDock.DTO.Dtos;
using GameDock.DTO.Interfaces;
using GameDock.Shared.Mappers;
using Moq;
using Xunit;

namespace GameDock.Tests.UnitTests
{
    public class StatisticUnitTests
    {
        private readonly Mock<IStatisticDAL> _statisticDalMock;
        private readonly StatisticContainer _container;
        private static readonly DateTime DefaultDate =
            new DateTime(2026, 4, 22, 10, 0, 0, DateTimeKind.Utc);

        public StatisticUnitTests()
        {
            _statisticDalMock = new Mock<IStatisticDAL>();
            _container = new StatisticContainer(_statisticDalMock.Object);
        }

        private static Statistic BuildValidStatistic(
            int statisticId = 1,
            int userId = 1,
            StatisticType statisticType = StatisticType.Coins,
            float value = 10,
            DateTime? createdAt = null)
        {
            return new Statistic(
                statisticId,
                userId,
                statisticType,
                value,
                createdAt ?? DefaultDate);
        }

        private static StatisticDto BuildValidStatisticDto(
            int statisticId = 1,
            int userId = 1,
            StatisticType statisticType = StatisticType.Coins,
            float value = 10,
            DateTime? createdAt = null)
        {
            return StatisticMapper.ToStatisticDto(
                BuildValidStatistic(statisticId, userId, statisticType, value, createdAt));
        }

        [Fact]
        public void TC01_STATISTIC_C_CreateStatistic_ValidStatistic_CallsCreateStatisticOnce()
        {
            var statistic = BuildValidStatistic();
            var dto = BuildValidStatisticDto();

            _statisticDalMock
                .Setup(x => x.CreateStatistic(It.IsAny<StatisticDto>()))
                .Returns(dto);

            var result = _container.CreateStatistic(statistic);

            Assert.NotNull(result);
            Assert.Equal(statistic.StatisticId, result.StatisticId);
            Assert.Equal(statistic.UserId, result.UserId);
            Assert.Equal(statistic.StatisticType, result.StatisticType);
            Assert.Equal(statistic.Value, result.Value);
            Assert.Equal(statistic.CreatedAt, result.CreatedAt);

            _statisticDalMock.Verify(
                x => x.CreateStatistic(It.Is<StatisticDto>(s =>
                    s.StatisticId == statistic.StatisticId &&
                    s.UserId == statistic.UserId &&
                    s.StatisticType == (GameDock.DTO.Dtos.Enums.StatisticTypeDto)statistic.StatisticType &&
                    s.Value == statistic.Value &&
                    s.CreatedAt == statistic.CreatedAt)),
                Times.Once);
        }

        [Fact]
        public void TC02_STATISTIC_C_CreateStatistic_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _container.CreateStatistic(null!));

            _statisticDalMock.Verify(
                x => x.CreateStatistic(It.IsAny<StatisticDto>()),
                Times.Never);
        }

        [Fact]
        public void TC03_STATISTIC_C_CreateStatistic_InvalidStatisticId_ThrowsArgumentException()
        {
            var statistic = BuildValidStatistic(statisticId: -1);

            var ex = Assert.Throws<ArgumentException>(() => _container.CreateStatistic(statistic));

            Assert.Equal("Statistic ID cannot be less than 0", ex.Message);

            _statisticDalMock.Verify(
                x => x.CreateStatistic(It.IsAny<StatisticDto>()),
                Times.Never);
        }

        [Fact]
        public void TC04_STATISTIC_C_CreateStatistic_InvalidStatisticType_ThrowsArgumentException()
        {
            var statistic = BuildValidStatistic(statisticType: (StatisticType)999);

            var ex = Assert.Throws<ArgumentException>(() => _container.CreateStatistic(statistic));

            Assert.Equal("StatisticType is invalid", ex.Message);

            _statisticDalMock.Verify(
                x => x.CreateStatistic(It.IsAny<StatisticDto>()),
                Times.Never);
        }

        [Fact]
        public void TC05_STATISTIC_C_CreateStatistic_InvalidUserId_ThrowsArgumentException()
        {
            var statistic = BuildValidStatistic(userId: -1);

            var ex = Assert.Throws<ArgumentException>(() => _container.CreateStatistic(statistic));

            Assert.Equal("User ID cannot be less than 0", ex.Message);

            _statisticDalMock.Verify(
                x => x.CreateStatistic(It.IsAny<StatisticDto>()),
                Times.Never);
        }

        [Fact]
        public void TC06_STATISTIC_C_CreateStatistic_BoundaryEnum_CreatesStatisticCorrectly()
        {
            var statistic = BuildValidStatistic(statisticType: StatisticType.Deaths);
            var dto = BuildValidStatisticDto(statisticType: StatisticType.Deaths);

            _statisticDalMock
                .Setup(x => x.CreateStatistic(It.IsAny<StatisticDto>()))
                .Returns(dto);

            var result = _container.CreateStatistic(statistic);

            Assert.NotNull(result);
            Assert.Equal(StatisticType.Deaths, result.StatisticType);

            _statisticDalMock.Verify(
                x => x.CreateStatistic(It.IsAny<StatisticDto>()),
                Times.Once);
        }

        [Fact]
        public void TC07_STATISTIC_R_ReadStatistic_ValidId_ReturnsStatistic()
        {
            var dto = BuildValidStatisticDto(
                statisticId: 5,
                userId: 2,
                statisticType: StatisticType.Kills,
                value: 25);

            _statisticDalMock
                .Setup(x => x.ReadStatistic(5))
                .Returns(dto);

            var result = _container.ReadStatistic(5);

            Assert.NotNull(result);
            Assert.Equal(5, result.StatisticId);
            Assert.Equal(2, result.UserId);
            Assert.Equal(StatisticType.Kills, result.StatisticType);
            Assert.Equal(25, result.Value);

            _statisticDalMock.Verify(x => x.ReadStatistic(5), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TC08_STATISTIC_R_ReadStatistic_InvalidId_ThrowsArgumentException(int id)
        {
            var ex = Assert.Throws<ArgumentException>(() => _container.ReadStatistic(id));

            Assert.Equal("ID cannot be 0 or negative", ex.Message);

            _statisticDalMock.Verify(
                x => x.ReadStatistic(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public void TC09_STATISTIC_U_UpdateStatistic_ValidId_CallsUpdateStatisticOnce()
        {
            var statistic = BuildValidStatistic(statisticId: 3);

            _statisticDalMock
                .Setup(x => x.ReadStatistic(3))
                .Returns(BuildValidStatisticDto(statisticId: 3));

            _container.UpdateStatistic(statistic);

            _statisticDalMock.Verify(x => x.ReadStatistic(3), Times.Once);
            _statisticDalMock.Verify(
                x => x.UpdateStatistic(It.Is<StatisticDto>(s =>
                    s.StatisticId == statistic.StatisticId &&
                    s.UserId == statistic.UserId &&
                    s.StatisticType == (GameDock.DTO.Dtos.Enums.StatisticTypeDto)statistic.StatisticType &&
                    s.Value == statistic.Value &&
                    s.CreatedAt == statistic.CreatedAt)),
                Times.Once);
        }

        [Fact]
        public void TC010_STATISTIC_U_UpdateStatistic_InvalidId_ThrowsArgumentException()
        {
            var statistic = BuildValidStatistic(statisticId: -1);

            var ex = Assert.Throws<ArgumentException>(() => _container.UpdateStatistic(statistic));

            Assert.Equal("Statistic ID cannot be less than 0", ex.Message);

            _statisticDalMock.Verify(
                x => x.ReadStatistic(It.IsAny<int>()),
                Times.Never);

            _statisticDalMock.Verify(
                x => x.UpdateStatistic(It.IsAny<StatisticDto>()),
                Times.Never);
        }

        [Fact]
        public void TC011_STATISTIC_U_UpdateStatistic_InvalidEnum_ThrowsArgumentException()
        {
            var statistic = BuildValidStatistic(statisticType: (StatisticType)999);

            var ex = Assert.Throws<ArgumentException>(() => _container.UpdateStatistic(statistic));

            Assert.Equal("StatisticType is invalid", ex.Message);

            _statisticDalMock.Verify(
                x => x.UpdateStatistic(It.IsAny<StatisticDto>()),
                Times.Never);
        }

        [Fact]
        public void TC012_STATISTIC_D_DeleteStatistic_ValidId_CallsDeleteStatisticOnce()
        {
            _statisticDalMock
                .Setup(x => x.ReadStatistic(4))
                .Returns(BuildValidStatisticDto(statisticId: 4));

            _container.DeleteStatistic(4);

            _statisticDalMock.Verify(x => x.ReadStatistic(4), Times.Once);
            _statisticDalMock.Verify(x => x.DeleteStatistic(4), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TC013_STATISTIC_D_DeleteStatistic_InvalidId_ThrowsArgumentException(int id)
        {
            var ex = Assert.Throws<ArgumentException>(() => _container.DeleteStatistic(id));

            Assert.Equal("ID cannot be 0 or negative", ex.Message);

            _statisticDalMock.Verify(
                x => x.ReadStatistic(It.IsAny<int>()),
                Times.Never);

            _statisticDalMock.Verify(
                x => x.DeleteStatistic(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public void TC014_STATISTIC_GA_GetAllStatistics_ReturnsList()
        {
            var dtos = new List<StatisticDto>
            {
                BuildValidStatisticDto(statisticId: 1, userId: 10, statisticType: StatisticType.Coins, value: 100),
                BuildValidStatisticDto(statisticId: 2, userId: 20, statisticType: StatisticType.Kills, value: 50)
            };

            _statisticDalMock
                .Setup(x => x.GetAllStatistics())
                .Returns(dtos);

            var result = _container.GetAllStatistics();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.StatisticId == 1 && x.UserId == 10 && x.StatisticType == StatisticType.Coins);
            Assert.Contains(result, x => x.StatisticId == 2 && x.UserId == 20 && x.StatisticType == StatisticType.Kills);

            _statisticDalMock.Verify(x => x.GetAllStatistics(), Times.Once);
        }
    }
}