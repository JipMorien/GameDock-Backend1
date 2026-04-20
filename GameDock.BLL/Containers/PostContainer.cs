using DTO.Interfaces;
using Domain;
using Shared.Mappers;

namespace BLL.Containers
{
    public class PostContainer
    {
        private readonly IPostDAL _postDAL;

        public PostContainer(IPostDAL postDAL)
        {
            _postDAL = postDAL ?? throw new ArgumentNullException(nameof(postDAL));
        }

        public void CheckPost(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            if (post.PostId < 0)
                throw new ArgumentException("Post ID cannot be less than 0");

            if (string.IsNullOrWhiteSpace(post.Content))
                throw new ArgumentException("Post content cannot be empty");

            if (post.UserId < 0)
                throw new ArgumentException("User ID cannot be less than 0");

            if (post.CreatedAt == default)
                throw new ArgumentException("CreatedAt must be valid");
        }

        public Post CreatePost(Post post)
        {
            CheckPost(post);

            var newPostDto = _postDAL.CreatePost(PostMapper.ToPostDto(post));

            if (newPostDto == null)
                throw new Exception("Post could not be created");

            return PostMapper.FromPostDto(newPostDto);
        }

        public Post ReadPost(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be 0 or negative");

            var postDto = _postDAL.ReadPost(id);

            if (postDto == null)
                throw new ArgumentException("Post could not be read");

            return PostMapper.FromPostDto(postDto);
        }

        public void UpdatePost(Post post)
        {
            CheckPost(post);

            var existingPost = _postDAL.ReadPost(post.PostId);
            if (existingPost == null)
                throw new ArgumentException("Post could not be read");

            _postDAL.UpdatePost(PostMapper.ToPostDto(post));
        }

        public void DeletePost(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be 0 or negative");

            var existingPost = _postDAL.ReadPost(id);
            if (existingPost == null)
                throw new ArgumentException("Post could not be read");

            _postDAL.DeletePost(id);
        }

        public List<Post> GetAllPosts()
        {
            var postDtos = _postDAL.GetAllPosts();

            return postDtos.Select(PostMapper.FromPostDto).ToList();
        }
    }
}