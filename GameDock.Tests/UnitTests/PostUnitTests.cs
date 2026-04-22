using GameDock.BLL.Containers;
using GameDock.Domain;
using GameDock.DTO.Dtos;
using GameDock.DTO.Interfaces;
using GameDock.Shared.Mappers;
using Moq;
using Xunit;

namespace GameDock.Tests.UnitTests
{
    public class PostUnitTests
    {
        private readonly Mock<IPostDAL> _postDalMock;
        private readonly PostContainer _container;
        private static readonly DateTime DefaultDate =
            new DateTime(2026, 4, 22, 10, 0, 0, DateTimeKind.Utc);

        public PostUnitTests()
        {
            _postDalMock = new Mock<IPostDAL>();
            _container = new PostContainer(_postDalMock.Object);
        }

        private static Post BuildValidPost(
            int postId = 1,
            string title = "Title",
            string content = "Content",
            DateTime? createdAt = null,
            int userId = 1)
        {
            return new Post(
                postId,
                title,
                content,
                createdAt ?? DefaultDate,
                userId);
        }

        private static PostDto BuildValidPostDto(
            int postId = 1,
            string title = "Title",
            string content = "Content",
            DateTime? createdAt = null,
            int userId = 1)
        {
            return PostMapper.ToPostDto(
                BuildValidPost(postId, title, content, createdAt, userId));
        }

        [Fact]
        public void TC01_POST_C_CreatePost_ValidPost_CallsCreatePostOnce()
        {
            var post = BuildValidPost();
            var dto = BuildValidPostDto();

            _postDalMock
                .Setup(x => x.CreatePost(It.IsAny<PostDto>()))
                .Returns(dto);

            var result = _container.CreatePost(post);

            Assert.NotNull(result);
            Assert.Equal(post.PostId, result.PostId);
            Assert.Equal(post.Title, result.Title);
            Assert.Equal(post.Content, result.Content);
            Assert.Equal(post.CreatedAt, result.CreatedAt);
            Assert.Equal(post.UserId, result.UserId);

            _postDalMock.Verify(
                x => x.CreatePost(It.Is<PostDto>(p =>
                    p.PostId == post.PostId &&
                    p.Title == post.Title &&
                    p.Content == post.Content &&
                    p.CreatedAt == post.CreatedAt &&
                    p.UserId == post.UserId)),
                Times.Once);
        }

        [Fact]
        public void TC02_POST_C_CreatePost_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _container.CreatePost(null!));

            _postDalMock.Verify(x => x.CreatePost(It.IsAny<PostDto>()), Times.Never);
        }

        [Fact]
        public void TC03_POST_C_CreatePost_InvalidId_ThrowsArgumentException()
        {
            var post = BuildValidPost(postId: -1);

            var ex = Assert.Throws<ArgumentException>(() => _container.CreatePost(post));

            Assert.Equal("Post ID cannot be less than 0", ex.Message);

            _postDalMock.Verify(x => x.CreatePost(It.IsAny<PostDto>()), Times.Never);
        }

        [Fact]
        public void TC04_POST_C_CreatePost_EmptyTitleOrContent_ThrowsArgumentException()
        {
            var post = BuildValidPost(title: null!, content: "");

            var ex = Assert.Throws<ArgumentException>(() => _container.CreatePost(post));

            Assert.True(
                ex.Message == "Post Title cannot be null" ||
                ex.Message == "Post content cannot be empty");

            _postDalMock.Verify(x => x.CreatePost(It.IsAny<PostDto>()), Times.Never);
        }

        [Fact]
        public void TC05_POST_C_CreatePost_InvalidUserId_ThrowsArgumentException()
        {
            var post = BuildValidPost(userId: -1);

            var ex = Assert.Throws<ArgumentException>(() => _container.CreatePost(post));

            Assert.Equal("User ID cannot be less than 0", ex.Message);

            _postDalMock.Verify(x => x.CreatePost(It.IsAny<PostDto>()), Times.Never);
        }

        [Fact]
        public void TC06_POST_R_ReadPost_ValidId_ReturnsPost()
        {
            var dto = BuildValidPostDto(postId: 5, title: "Read Title", content: "Read Content", userId: 2);

            _postDalMock
                .Setup(x => x.ReadPost(5))
                .Returns(dto);

            var result = _container.ReadPost(5);

            Assert.NotNull(result);
            Assert.Equal(5, result.PostId);
            Assert.Equal("Read Title", result.Title);
            Assert.Equal("Read Content", result.Content);
            Assert.Equal(2, result.UserId);

            _postDalMock.Verify(x => x.ReadPost(5), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TC07_POST_R_ReadPost_InvalidId_ThrowsArgumentException(int id)
        {
            var ex = Assert.Throws<ArgumentException>(() => _container.ReadPost(id));

            Assert.Equal("ID cannot be 0 or negative", ex.Message);

            _postDalMock.Verify(x => x.ReadPost(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void TC08_POST_U_UpdatePost_ValidPost_CallsUpdatePostOnce()
        {
            var post = BuildValidPost(postId: 3);

            _postDalMock
                .Setup(x => x.ReadPost(3))
                .Returns(BuildValidPostDto(postId: 3));

            _container.UpdatePost(post);

            _postDalMock.Verify(x => x.ReadPost(3), Times.Once);
            _postDalMock.Verify(
                x => x.UpdatePost(It.Is<PostDto>(p =>
                    p.PostId == post.PostId &&
                    p.Title == post.Title &&
                    p.Content == post.Content &&
                    p.CreatedAt == post.CreatedAt &&
                    p.UserId == post.UserId)),
                Times.Once);
        }

        [Fact]
        public void TC09_POST_U_UpdatePost_InvalidId_ThrowsArgumentException()
        {
            var post = BuildValidPost(postId: -1);

            var ex = Assert.Throws<ArgumentException>(() => _container.UpdatePost(post));

            Assert.Equal("Post ID cannot be less than 0", ex.Message);

            _postDalMock.Verify(x => x.UpdatePost(It.IsAny<PostDto>()), Times.Never);
        }

        [Fact]
        public void TC010_POST_U_UpdatePost_EmptyTitleOrContent_ThrowsArgumentException()
        {
            var post = BuildValidPost(title: null!, content: "");

            var ex = Assert.Throws<ArgumentException>(() => _container.UpdatePost(post));

            Assert.True(
                ex.Message == "Post Title cannot be null" ||
                ex.Message == "Post content cannot be empty");

            _postDalMock.Verify(x => x.UpdatePost(It.IsAny<PostDto>()), Times.Never);
        }

        [Fact]
        public void TC011_POST_D_DeletePost_ValidId_CallsDeletePostOnce()
        {
            _postDalMock
                .Setup(x => x.ReadPost(4))
                .Returns(BuildValidPostDto(postId: 4));

            _container.DeletePost(4);

            _postDalMock.Verify(x => x.ReadPost(4), Times.Once);
            _postDalMock.Verify(x => x.DeletePost(4), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TC012_POST_D_DeletePost_InvalidId_ThrowsArgumentException(int id)
        {
            var ex = Assert.Throws<ArgumentException>(() => _container.DeletePost(id));

            Assert.Equal("ID cannot be 0 or negative", ex.Message);

            _postDalMock.Verify(x => x.DeletePost(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void TC013_POST_GA_GetAllPosts_ReturnsList()
        {
            var dtos = new List<PostDto>
            {
                BuildValidPostDto(postId: 1, title: "Post 1"),
                BuildValidPostDto(postId: 2, title: "Post 2")
            };

            _postDalMock
                .Setup(x => x.GetAllPosts())
                .Returns(dtos);

            var result = _container.GetAllPosts();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.PostId == 1 && x.Title == "Post 1");
            Assert.Contains(result, x => x.PostId == 2 && x.Title == "Post 2");

            _postDalMock.Verify(x => x.GetAllPosts(), Times.Once);
        }
    }
}