using Domain;
using DTO.Dtos;

namespace Shared.Mappers
{
    public static class PostMapper
    {
        public static Post FromPostDto(PostDto dto)
        {
            if (dto == null) 
                throw new ArgumentNullException(nameof(dto));

            return new Post(
                dto.PostId,
                dto.Content,
                dto.CreatedAt,
                dto.UserId);
        }
        
        public static PostDto ToPostDto(Post post)
        {
            if (post == null) 
                throw new ArgumentNullException(nameof(post));

            return new PostDto
            {
                PostId = post.PostId,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UserId = post.UserId
            };
        }
        
        
        
        
    
    }
}