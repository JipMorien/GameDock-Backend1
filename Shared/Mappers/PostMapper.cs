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
        
        public static Post ToPostDto(Post post)
        {
            if (post == null) 
                throw new ArgumentNullException(nameof(post));

            return new Post(
                post.PostId,
                post.Content,
                post.CreatedAt,
                post.UserId);
        }
        
        
        
        
    
    }
}