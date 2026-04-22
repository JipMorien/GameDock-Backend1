using GameDock.BLL.Containers;
using GameDock.Domain;
using GameDock.DTO.Dtos;
using GameDock.DTO.Interfaces;
using GameDock.Shared.Mappers;
using Moq;
using Xunit;

namespace GameDock.Tests.UnitTests
{
    public class LeaderboardUnitTests
    {
        private readonly Mock<ILeaderboardDAL> _leaderboardDalMock;
        private readonly LeaderboardContainer _container;

        public LeaderboardUnitTests()
        {
            _leaderboardDalMock = new Mock<ILeaderboardDAL>();
            _container = new LeaderboardContainer(_leaderboardDalMock.Object);
        }

        private static Leaderboard BuildValidLeaderboard(
            int leaderboardId = 1,
            string name = "Top Players",
            int userId = 1)
        {
            return new Leaderboard(leaderboardId, name, userId);
        }

        private static LeaderboardDto BuildValidLeaderboardDto(
            int leaderboardId = 1,
            string name = "Top Players",
            int userId = 1)
        {
            return LeaderboardMapper.ToLeaderboardDto(
                BuildValidLeaderboard(leaderboardId, name, userId));
        }

        [Fact]
        public void TC01_LEADERBOARD_C_CreateLeaderboard_ValidLeaderboard_ReturnsMappedLeaderboard()
        {
            // Arrange
            var leaderboard = BuildValidLeaderboard();
            var dto = BuildValidLeaderboardDto();

            _leaderboardDalMock
                .Setup(dal => dal.CreateLeaderboard(It.IsAny<LeaderboardDto>()))
                .Returns(dto);

            // Act
            var result = _container.CreateLeaderboard(leaderboard);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(leaderboard.LeaderboardId, result.LeaderboardId);
            Assert.Equal(leaderboard.Name, result.Name);
            Assert.Equal(leaderboard.UserId, result.UserId);

            _leaderboardDalMock.Verify(
                dal => dal.CreateLeaderboard(It.Is<LeaderboardDto>(x =>
                    x.LeaderboardId == leaderboard.LeaderboardId &&
                    x.Name == leaderboard.Name &&
                    x.UserId == leaderboard.UserId)),
                Times.Once);
        }

        [Fact]
        public void TC02_LEADERBOARD_C_CreateLeaderboard_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _container.CreateLeaderboard(null!));

            _leaderboardDalMock.Verify(
                dal => dal.CreateLeaderboard(It.IsAny<LeaderboardDto>()),
                Times.Never);
        }

        [Fact]
        public void TC03_LEADERBOARD_C_CreateLeaderboard_InvalidLeaderboardId_ThrowsArgumentException()
        {
            // Arrange
            var leaderboard = BuildValidLeaderboard(leaderboardId: -1);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.CreateLeaderboard(leaderboard));

            // Assert
            Assert.Equal("Leaderboard ID cannot be less than 0", ex.Message);

            _leaderboardDalMock.Verify(
                dal => dal.CreateLeaderboard(It.IsAny<LeaderboardDto>()),
                Times.Never);
        }

        [Fact]
        public void TC04_LEADERBOARD_C_CreateLeaderboard_EmptyName_ThrowsArgumentException()
        {
            // Arrange
            var leaderboard = BuildValidLeaderboard(name: "");

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.CreateLeaderboard(leaderboard));

            // Assert
            Assert.Equal("Leaderboard name cannot be empty", ex.Message);

            _leaderboardDalMock.Verify(
                dal => dal.CreateLeaderboard(It.IsAny<LeaderboardDto>()),
                Times.Never);
        }

        [Fact]
        public void TC05_LEADERBOARD_C_CreateLeaderboard_InvalidUserId_ThrowsArgumentException()
        {
            // Arrange
            var leaderboard = BuildValidLeaderboard(userId: -1);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.CreateLeaderboard(leaderboard));

            // Assert
            Assert.Equal("User ID cannot be less than 0", ex.Message);

            _leaderboardDalMock.Verify(
                dal => dal.CreateLeaderboard(It.IsAny<LeaderboardDto>()),
                Times.Never);
        }

        [Fact]
        public void TC06_LEADERBOARD_R_ReadLeaderboard_ValidId_ReturnsMappedLeaderboard()
        {
            // Arrange
            var dto = BuildValidLeaderboardDto(leaderboardId: 5, name: "Leaderboard 5", userId: 2);

            _leaderboardDalMock
                .Setup(dal => dal.ReadLeaderboard(5))
                .Returns(dto);

            // Act
            var result = _container.ReadLeaderboard(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.LeaderboardId);
            Assert.Equal("Leaderboard 5", result.Name);
            Assert.Equal(2, result.UserId);

            _leaderboardDalMock.Verify(dal => dal.ReadLeaderboard(5), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TC07_LEADERBOARD_R_ReadLeaderboard_InvalidId_ThrowsArgumentException(int id)
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.ReadLeaderboard(id));

            // Assert
            Assert.Equal("ID can't be negative", ex.Message);

            _leaderboardDalMock.Verify(
                dal => dal.ReadLeaderboard(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public void TC08_LEADERBOARD_R_ReadLeaderboard_NotFound_ThrowsArgumentException()
        {
            // Arrange
            _leaderboardDalMock
                .Setup(dal => dal.ReadLeaderboard(1))
                .Returns((LeaderboardDto?)null);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.ReadLeaderboard(1));

            // Assert
            Assert.Equal("Leaderboard could not be read", ex.Message);

            _leaderboardDalMock.Verify(dal => dal.ReadLeaderboard(1), Times.Once);
        }

        [Fact]
        public void TC09_LEADERBOARD_U_UpdateLeaderboard_ValidLeaderboard_CallsUpdateLeaderboardOnce()
        {
            // Arrange
            var leaderboard = BuildValidLeaderboard(leaderboardId: 3);

            _leaderboardDalMock
                .Setup(dal => dal.ReadLeaderboard(3))
                .Returns(BuildValidLeaderboardDto(leaderboardId: 3));

            // Act
            _container.UpdateLeaderboard(leaderboard);

            // Assert
            _leaderboardDalMock.Verify(dal => dal.ReadLeaderboard(3), Times.Once);

            _leaderboardDalMock.Verify(
                dal => dal.UpdateLeaderboard(It.Is<LeaderboardDto>(x =>
                    x.LeaderboardId == leaderboard.LeaderboardId &&
                    x.Name == leaderboard.Name &&
                    x.UserId == leaderboard.UserId)),
                Times.Once);
        }

        [Fact]
        public void TC010_LEADERBOARD_U_UpdateLeaderboard_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var leaderboard = BuildValidLeaderboard(leaderboardId: -1);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.UpdateLeaderboard(leaderboard));

            // Assert
            Assert.Equal("Leaderboard ID cannot be less than 0", ex.Message);

            _leaderboardDalMock.Verify(
                dal => dal.ReadLeaderboard(It.IsAny<int>()),
                Times.Never);

            _leaderboardDalMock.Verify(
                dal => dal.UpdateLeaderboard(It.IsAny<LeaderboardDto>()),
                Times.Never);
        }

        [Fact]
        public void TC011_LEADERBOARD_D_DeleteLeaderboard_ValidId_CallsDeleteLeaderboardOnce()
        {
            // Arrange
            _leaderboardDalMock
                .Setup(dal => dal.ReadLeaderboard(4))
                .Returns(BuildValidLeaderboardDto(leaderboardId: 4));

            // Act
            _container.DeleteLeaderboard(4);

            // Assert
            _leaderboardDalMock.Verify(dal => dal.ReadLeaderboard(4), Times.Once);
            _leaderboardDalMock.Verify(dal => dal.DeleteLeaderboard(4), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TC012_LEADERBOARD_D_DeleteLeaderboard_InvalidId_ThrowsArgumentException(int id)
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.DeleteLeaderboard(id));

            // Assert
            Assert.Equal("ID can't be negative", ex.Message);

            _leaderboardDalMock.Verify(
                dal => dal.ReadLeaderboard(It.IsAny<int>()),
                Times.Never);

            _leaderboardDalMock.Verify(
                dal => dal.DeleteLeaderboard(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public void TC013_LEADERBOARD_GA_GetAllLeaderboards_ReturnsMappedList()
        {
            // Arrange
            var dtos = new List<LeaderboardDto>
            {
                BuildValidLeaderboardDto(leaderboardId: 1, name: "Leaderboard 1", userId: 10),
                BuildValidLeaderboardDto(leaderboardId: 2, name: "Leaderboard 2", userId: 20)
            };

            _leaderboardDalMock
                .Setup(dal => dal.GetAllLeaderboards())
                .Returns(dtos);

            // Act
            var result = _container.GetAllLeaderboards();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.LeaderboardId == 1 && x.Name == "Leaderboard 1" && x.UserId == 10);
            Assert.Contains(result, x => x.LeaderboardId == 2 && x.Name == "Leaderboard 2" && x.UserId == 20);

            _leaderboardDalMock.Verify(dal => dal.GetAllLeaderboards(), Times.Once);
        }
    }
}