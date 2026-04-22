using GameDock.BLL.Containers;
using GameDock.Domain;
using GameDock.DTO.Dtos;
using GameDock.DTO.Interfaces;
using GameDock.Shared.Mappers;
using Moq;
using Xunit;

namespace GameDock.Tests.UnitTests
{
    public class GameDockUserUnitTests
    {
        private readonly Mock<IGameDockUserDAL> _gameDockUserDalMock;
        private readonly GameDockUserContainer _container;
        private static readonly DateTime DefaultDate = new DateTime(2026, 4, 22, 10, 0, 0, DateTimeKind.Utc);
        public GameDockUserUnitTests()
        {
            _gameDockUserDalMock = new Mock<IGameDockUserDAL>();
            _container = new GameDockUserContainer(_gameDockUserDalMock.Object);
        }

        private static GameDockUser BuildValidGameDockUser(
            int id = 1,
            bool isAdmin = false,
            string userName = "Jip",
            string email = "jip@test.com",
            string passwordHash = "hashed-password",
            DateTime? createdAt = null)
        {
            return new GameDockUser(
                isAdmin,
                id,
                userName,
                email,
                passwordHash,
                createdAt ?? DefaultDate);
        }

        private static GameDockUserDto BuildValidGameDockUserDto(
            int id = 1,
            bool isAdmin = false,
            string userName = "Jip",
            string email = "jip@test.com",
            string passwordHash = "hashed-password",
            DateTime? createdAt = null)
        {
            var user = BuildValidGameDockUser(id, isAdmin, userName, email, passwordHash, createdAt);
            return GameDockUserMapper.ToUserDto(user);
        }
        
        
        [Fact]
        public void TC01_USER_C_CreateUser_ValidUser_ReturnsMappedUser()
        {
            // Arrange
            var user = BuildValidGameDockUser();
            var dto = BuildValidGameDockUserDto();

            _gameDockUserDalMock
                .Setup(dal => dal.CreateUser(It.IsAny<GameDockUserDto>()))
                .Returns(dto);

            // Act
            var result = _container.CreateUser(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.GameDockUserId, result.GameDockUserId);
            Assert.Equal(user.IsAdmin, result.IsAdmin);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.PasswordHash, result.PasswordHash);
            Assert.Equal(user.CreatedAt, result.CreatedAt);

            _gameDockUserDalMock.Verify(
                dal => dal.CreateUser(It.Is<GameDockUserDto>(x =>
                    x.GameDockUserId == user.GameDockUserId &&
                    x.IsAdmin == user.IsAdmin &&
                    x.UserName == user.UserName &&
                    x.Email == user.Email &&
                    x.PasswordHash == user.PasswordHash &&
                    x.CreatedAt == user.CreatedAt)),
                Times.Once);
        }
        
        [Fact]
        public void TC02_USER_C_CreateUser_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _container.CreateUser(null!));

            _gameDockUserDalMock.Verify(
                dal => dal.CreateUser(It.IsAny<GameDockUserDto>()),
                Times.Never);
        }

        [Fact]
        public void TC03_USER_C_CreateUser_EmptyUsername_ThrowsArgumentException()
        {
            // Arrange
            var user = BuildValidGameDockUser(userName: "");

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.CreateUser(user));

            // Assert
            Assert.Equal("Username cannot be empty.", ex.Message);

            _gameDockUserDalMock.Verify(
                dal => dal.CreateUser(It.IsAny<GameDockUserDto>()),
                Times.Never);
        }
        
        [Fact]
        public void TC04_USER_C_CreateUser_InvalidEmail_ThrowsArgumentException()
        {
            // Arrange
            var user = BuildValidGameDockUser(email: "invalid-email");

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.CreateUser(user));

            // Assert
            Assert.Equal("Email must be valid.", ex.Message);

            _gameDockUserDalMock.Verify(
                dal => dal.CreateUser(It.IsAny<GameDockUserDto>()),
                Times.Never);
        }
        
        [Fact]
        public void TC05_USER_C_CreateUser_DalReturnsNull_ThrowsArgumentException()
        {
            // Arrange
            var user = BuildValidGameDockUser();

            _gameDockUserDalMock
                .Setup(dal => dal.CreateUser(It.IsAny<GameDockUserDto>()))
                .Returns((GameDockUserDto)null!);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.CreateUser(user));

            // Assert
            Assert.Equal("GameDockUserDto cannot be null", ex.Message);

            _gameDockUserDalMock.Verify(
                dal => dal.CreateUser(It.IsAny<GameDockUserDto>()),
                Times.Once);
        }
        
        [Fact]
        public void TC06_USER_R_ReadUser_ValidId_ReturnsMappedUser()
        {
            // Arrange
            var dto = BuildValidGameDockUserDto(
                id: 5,
                userName: "TestUser",
                email: "testuser@test.com");

            _gameDockUserDalMock
                .Setup(dal => dal.ReadUser(5))
                .Returns(dto);

            // Act
            var result = _container.ReadUser(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.GameDockUserId);
            Assert.Equal(dto.IsAdmin, result.IsAdmin);
            Assert.Equal("TestUser", result.UserName);
            Assert.Equal("testuser@test.com", result.Email);
            Assert.Equal(dto.PasswordHash, result.PasswordHash);
            Assert.Equal(dto.CreatedAt, result.CreatedAt);

            _gameDockUserDalMock.Verify(dal => dal.ReadUser(5), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TC07_USER_R_ReadUser_IdLessThanOrEqualZero_ThrowsArgumentException(int id)
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.ReadUser(id));

            // Assert
            Assert.Equal("ID cannot be negative.", ex.Message);

            _gameDockUserDalMock.Verify(
                dal => dal.ReadUser(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public void TC08_USER_R_ReadUser_UserDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            _gameDockUserDalMock
                .Setup(dal => dal.ReadUser(1))
                .Returns((GameDockUserDto?)null);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.ReadUser(1));

            // Assert
            Assert.Equal("GameDockUserDto cannot be null", ex.Message);

            _gameDockUserDalMock.Verify(dal => dal.ReadUser(1), Times.Once);
        }
        
        [Fact]
        public void TC09_USER_U_UpdateUser_ExistingValidUser_CallsUpdateUserOnce()
        {
            // Arrange
            var user = BuildValidGameDockUser(id: 3);

            _gameDockUserDalMock
                .Setup(dal => dal.ReadUser(3))
                .Returns(BuildValidGameDockUserDto(id: 3));

            // Act
            _container.UpdateUser(user);

            // Assert
            _gameDockUserDalMock.Verify(dal => dal.ReadUser(3), Times.Once);

            _gameDockUserDalMock.Verify(
                dal => dal.UpdateUser(It.Is<GameDockUserDto>(x =>
                    x.GameDockUserId == user.GameDockUserId &&
                    x.IsAdmin == user.IsAdmin &&
                    x.UserName == user.UserName &&
                    x.Email == user.Email &&
                    x.PasswordHash == user.PasswordHash &&
                    x.CreatedAt == user.CreatedAt)),
                Times.Once);
        }

        [Fact]
        public void TC010_USER_U_UpdateUser_UserDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var user = BuildValidGameDockUser(id: 3);

            _gameDockUserDalMock
                .Setup(dal => dal.ReadUser(3))
                .Returns((GameDockUserDto?)null);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.UpdateUser(user));

            // Assert
            Assert.Equal("GameDockUserDto could not be read", ex.Message);

            _gameDockUserDalMock.Verify(dal => dal.ReadUser(3), Times.Once);
            _gameDockUserDalMock.Verify(
                dal => dal.UpdateUser(It.IsAny<GameDockUserDto>()),
                Times.Never);
        }

        [Fact]
        public void TC011_USER_U_UpdateUser_InvalidUser_ThrowsArgumentException()
        {
            // Arrange
            var user = BuildValidGameDockUser(userName: "");

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _container.UpdateUser(user));

            // Assert
            Assert.Equal("Username cannot be empty.", ex.Message);

            _gameDockUserDalMock.Verify(
                dal => dal.ReadUser(It.IsAny<int>()),
                Times.Never);

            _gameDockUserDalMock.Verify(
                dal => dal.UpdateUser(It.IsAny<GameDockUserDto>()),
                Times.Never);
        }
        
        [Fact]
public void TC012_USER_D_DeleteUser_ExistingUser_CallsDeleteUserOnce()
{
    // Arrange
    _gameDockUserDalMock
        .Setup(dal => dal.ReadUser(4))
        .Returns(BuildValidGameDockUserDto(id: 4));

    // Act
    _container.DeleteUser(4);

    // Assert
    _gameDockUserDalMock.Verify(dal => dal.ReadUser(4), Times.Once);
    _gameDockUserDalMock.Verify(dal => dal.DeleteUser(4), Times.Once);
}

[Theory]
[InlineData(0)]
[InlineData(-1)]
public void TC013_USER_D_DeleteUser_IdLessThanOrEqualZero_ThrowsArgumentException(int id)
{
    // Act
    var ex = Assert.Throws<ArgumentException>(() => _container.DeleteUser(id));

    // Assert
    Assert.Equal("ID cannot be negative.", ex.Message);

    _gameDockUserDalMock.Verify(
        dal => dal.ReadUser(It.IsAny<int>()),
        Times.Never);

    _gameDockUserDalMock.Verify(
        dal => dal.DeleteUser(It.IsAny<int>()),
        Times.Never);
}

[Fact]
public void TC014_USER_D_DeleteUser_UserDoesNotExist_ThrowsArgumentException()
{
    // Arrange
    _gameDockUserDalMock
        .Setup(dal => dal.ReadUser(4))
        .Returns((GameDockUserDto?)null);

    // Act
    var ex = Assert.Throws<ArgumentException>(() => _container.DeleteUser(4));

    // Assert
    Assert.Equal("GameDockUserDto could not be read", ex.Message);

    _gameDockUserDalMock.Verify(dal => dal.ReadUser(4), Times.Once);
    _gameDockUserDalMock.Verify(
        dal => dal.DeleteUser(It.IsAny<int>()),
        Times.Never);
}

[Fact]
public void TC015_USER_GA_GetAllUsers_WithUsers_ReturnsMappedList()
{
    // Arrange
    var dtos = new List<GameDockUserDto>
    {
        BuildValidGameDockUserDto(id: 1, userName: "User1", email: "user1@test.com"),
        BuildValidGameDockUserDto(id: 2, userName: "User2", email: "user2@test.com")
    };

    _gameDockUserDalMock
        .Setup(dal => dal.GetAllUsers())
        .Returns(dtos);

    // Act
    var result = _container.GetAllUsers();

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count);
    Assert.Contains(result, x => x.GameDockUserId == 1 && x.UserName == "User1");
    Assert.Contains(result, x => x.GameDockUserId == 2 && x.UserName == "User2");

    _gameDockUserDalMock.Verify(dal => dal.GetAllUsers(), Times.Once);
}

[Fact]
public void TC016_USER_GA_GetAllUsers_NoUsers_ReturnsEmptyList()
{
    // Arrange
    _gameDockUserDalMock
        .Setup(dal => dal.GetAllUsers())
        .Returns(new List<GameDockUserDto>());

    // Act
    var result = _container.GetAllUsers();

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);

    _gameDockUserDalMock.Verify(dal => dal.GetAllUsers(), Times.Once);
}
    }
}

