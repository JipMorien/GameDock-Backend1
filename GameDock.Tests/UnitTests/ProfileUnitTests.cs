using GameDock.BLL.Containers;
using GameDock.Domain;
using GameDock.DTO.Dtos;
using GameDock.DTO.Interfaces;
using GameDock.Shared.Mappers;
using Moq;
using Xunit;

namespace GameDock.Tests.UnitTests
{
    public class ProfileUnitTests
    {
        private readonly Mock<IProfileDAL> _profileDalMock;
        private readonly ProfileContainer _container;

        public ProfileUnitTests()
        {
            _profileDalMock = new Mock<IProfileDAL>();
            _container = new ProfileContainer(_profileDalMock.Object);
        }

        private static Profile BuildValidProfile(
            int profileId = 1,
            string userName = "Jip",
            int userId = 1,
            string bio = "Test bio",
            int level = 1,
            DateTime? createdAt = null)
        {
            return new Profile(
                profileId,
                userName,
                userId,
                bio,
                level,
                createdAt ?? DateTime.UtcNow);
        }

        private static ProfileDto BuildValidProfileDto(
            int profileId = 1,
            string userName = "Jip",
            int userId = 1,
            string bio = "Test bio",
            int level = 1,
            DateTime? createdAt = null)
        {
            return ProfileMapper.ToProfileDto(
                BuildValidProfile(profileId, userName, userId, bio, level, createdAt));
        }

        [Fact]
        public void TC01_PROFILE_C_CreateProfile_ValidProfile_CallsCreateProfileOnce()
        {
            var profile = BuildValidProfile();
            var dto = BuildValidProfileDto();

            _profileDalMock
                .Setup(x => x.CreateProfile(It.IsAny<ProfileDto>()))
                .Returns(dto);

            var result = _container.CreateProfile(profile);

            Assert.NotNull(result);
            Assert.Equal(profile.ProfileId, result.ProfileId);
            Assert.Equal(profile.UserName, result.UserName);
            Assert.Equal(profile.UserId, result.UserId);

            _profileDalMock.Verify(
                x => x.CreateProfile(It.Is<ProfileDto>(p =>
                    p.ProfileId == profile.ProfileId &&
                    p.UserName == profile.UserName &&
                    p.UserId == profile.UserId)),
                Times.Once);
        }

        [Fact]
        public void TC02_PROFILE_C_CreateProfile_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _container.CreateProfile(null!));

            _profileDalMock.Verify(
                x => x.CreateProfile(It.IsAny<ProfileDto>()),
                Times.Never);
        }

        [Fact]
        public void TC03_PROFILE_C_CreateProfile_InvalidProfileId_ThrowsArgumentException()
        {
            var profile = BuildValidProfile(profileId: -1);

            var ex = Assert.Throws<ArgumentException>(() => _container.CreateProfile(profile));

            Assert.Equal("Profile ID cannot be less than 0", ex.Message);

            _profileDalMock.Verify(
                x => x.CreateProfile(It.IsAny<ProfileDto>()),
                Times.Never);
        }

        [Fact]
        public void TC04_PROFILE_C_CreateProfile_EmptyUsername_ThrowsArgumentException()
        {
            var profile = BuildValidProfile(userName: "");

            var ex = Assert.Throws<ArgumentException>(() => _container.CreateProfile(profile));

            Assert.Equal("Username cannot be empty", ex.Message);

            _profileDalMock.Verify(
                x => x.CreateProfile(It.IsAny<ProfileDto>()),
                Times.Never);
        }

        [Fact]
        public void TC05_PROFILE_R_ReadProfile_ValidId_ReturnsProfile()
        {
            var dto = BuildValidProfileDto(profileId: 5, userName: "ReadUser", userId: 2);

            _profileDalMock
                .Setup(x => x.ReadProfile(5))
                .Returns(dto);

            var result = _container.ReadProfile(5);

            Assert.NotNull(result);
            Assert.Equal(5, result.ProfileId);
            Assert.Equal("ReadUser", result.UserName);
            Assert.Equal(2, result.UserId);

            _profileDalMock.Verify(x => x.ReadProfile(5), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TC06_PROFILE_R_ReadProfile_InvalidId_ThrowsArgumentException(int id)
        {
            var ex = Assert.Throws<ArgumentException>(() => _container.ReadProfile(id));

            Assert.Equal("ID cannot be 0 or negative", ex.Message);

            _profileDalMock.Verify(
                x => x.ReadProfile(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public void TC07_PROFILE_U_UpdateProfile_ValidProfile_CallsUpdateProfileOnce()
        {
            var profile = BuildValidProfile(profileId: 3);

            _profileDalMock
                .Setup(x => x.ReadProfile(3))
                .Returns(BuildValidProfileDto(profileId: 3));

            _container.UpdateProfile(profile);

            _profileDalMock.Verify(x => x.ReadProfile(3), Times.Once);
            _profileDalMock.Verify(
                x => x.UpdateProfile(It.Is<ProfileDto>(p =>
                    p.ProfileId == profile.ProfileId &&
                    p.UserName == profile.UserName &&
                    p.UserId == profile.UserId)),
                Times.Once);
        }

        [Fact]
        public void TC08_PROFILE_U_UpdateProfile_InvalidProfileId_ThrowsArgumentException()
        {
            var profile = BuildValidProfile(profileId: -1);

            var ex = Assert.Throws<ArgumentException>(() => _container.UpdateProfile(profile));

            Assert.Equal("Profile ID cannot be less than 0", ex.Message);

            _profileDalMock.Verify(
                x => x.ReadProfile(It.IsAny<int>()),
                Times.Never);

            _profileDalMock.Verify(
                x => x.UpdateProfile(It.IsAny<ProfileDto>()),
                Times.Never);
        }

        [Fact]
        public void TC09_PROFILE_U_UpdateProfile_EmptyUsername_ThrowsArgumentException()
        {
            var profile = BuildValidProfile(userName: "");

            var ex = Assert.Throws<ArgumentException>(() => _container.UpdateProfile(profile));

            Assert.Equal("Username cannot be empty", ex.Message);

            _profileDalMock.Verify(
                x => x.ReadProfile(It.IsAny<int>()),
                Times.Never);

            _profileDalMock.Verify(
                x => x.UpdateProfile(It.IsAny<ProfileDto>()),
                Times.Never);
        }

        [Fact]
        public void TC010_PROFILE_D_DeleteProfile_ValidId_CallsDeleteProfileOnce()
        {
            _profileDalMock
                .Setup(x => x.ReadProfile(4))
                .Returns(BuildValidProfileDto(profileId: 4));

            _container.DeleteProfile(4);

            _profileDalMock.Verify(x => x.ReadProfile(4), Times.Once);
            _profileDalMock.Verify(x => x.DeleteProfile(4), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TC011_PROFILE_D_DeleteProfile_InvalidId_ThrowsArgumentException(int id)
        {
            var ex = Assert.Throws<ArgumentException>(() => _container.DeleteProfile(id));

            Assert.Equal("ID cannot be 0 or negative", ex.Message);

            _profileDalMock.Verify(
                x => x.ReadProfile(It.IsAny<int>()),
                Times.Never);

            _profileDalMock.Verify(
                x => x.DeleteProfile(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public void TC012_PROFILE_GA_GetAllProfiles_ReturnsList()
        {
            var dtos = new List<ProfileDto>
            {
                BuildValidProfileDto(profileId: 1, userName: "Profile1", userId: 10),
                BuildValidProfileDto(profileId: 2, userName: "Profile2", userId: 20)
            };

            _profileDalMock
                .Setup(x => x.GetAllProfiles())
                .Returns(dtos);

            var result = _container.GetAllProfiles();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.ProfileId == 1 && x.UserName == "Profile1" && x.UserId == 10);
            Assert.Contains(result, x => x.ProfileId == 2 && x.UserName == "Profile2" && x.UserId == 20);

            _profileDalMock.Verify(x => x.GetAllProfiles(), Times.Once);
        }
    }
}