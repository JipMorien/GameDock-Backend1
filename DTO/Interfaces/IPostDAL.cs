using DTO.Dtos;

namespace DTO.Interfaces
{
    public interface IPostDAL
    {
        PostDto CreatePost(PostDto post);
        PostDto ReadPost(int id);
        void UpdatePost(PostDto post);
        void DeletePost(int id);
        List<PostDto> GetAllPosts();

    }
}