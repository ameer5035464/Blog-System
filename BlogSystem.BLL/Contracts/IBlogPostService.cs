using BlogSystem.BLL.DtoModels.BlogPostDtos;
using BlogSystem.BLL.DtoModels.CategoriesDtos;
using BlogSystem.BLL.helpers;

namespace BlogSystem.BLL.Contracts
{
    public interface IBlogPostService
    {
        Task<PostDto> CreatePost(CreatePostDto postDto, string currentUser);
        Task<Pagination<PostDto>> GetPublishedPosts(int categoryId, int pageNumber);
        Task<Pagination<PostDto>> GetCurrentPublishedUserPosts(string userId, int categoryId,int pageNumber);
        Task<Pagination<PostDto>> GetCurrentDraftsUserPosts(string currentUser, int categoryId, int pageNumber);
        Task<Pagination<PostDto>> GetCurrentArchivedPosts(string authorId, int categoryId, int pageNumber);
        Task<PostDto> GetPostById(string currentUser,int postId);
        Task<PostDto> UpdatePost(UpdatePostDto updatePost, string currentUser);
        Task DeletePost(int postId, string currentUser);
    }
}
