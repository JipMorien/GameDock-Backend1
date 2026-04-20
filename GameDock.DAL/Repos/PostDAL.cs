using Shared.Mappers;
using DTO.Dtos;
using DTO.Interfaces;

namespace DAL.Repos
{
    public class PostDAL : IPostDAL
    {
        private readonly AppDbContext _context;

        public PostDAL(AppDbContext context)
        {
            _context = context;
        }

        public PostDto CreatePost(PostDto post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            
            var entity = PostMapper.FromPostDto(post);
            
            _context.Posts.Add(entity);
            _context.SaveChanges();
            
            return PostMapper.ToPostDto(entity);
        }

        public PostDto? ReadPost(int id)
        {
            var entity = _context.Posts.Find(id);
            
            if (entity == null)
                return null;
            
            return PostMapper.ToPostDto(entity);
        }

        public void UpdatePost(PostDto post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            
            var existingEntity = _context.Posts.Find(post.PostId);
            if (existingEntity == null)
                throw new Exception($"Post {post.PostId} not found");
            
            existingEntity.Content = post.Content;
            existingEntity.UserId = post.UserId;
            existingEntity.PostId = post.PostId;
            existingEntity.CreatedAt = post.CreatedAt;
            
            _context.SaveChanges();
        }

        public void DeletePost(int id)
        {
            var entity = _context.Posts.Find(id);
            if  (entity == null)   
                throw new Exception($"Post {id} not found");
            
            _context.Posts.Remove(entity);
            _context.SaveChanges();
        }

        public List<PostDto> GetAllPosts()
        {
            return _context.Posts.Select(PostMapper.ToPostDto).ToList();
        }
    }
}

