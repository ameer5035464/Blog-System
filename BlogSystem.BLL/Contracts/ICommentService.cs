using BlogSystem.BLL.DtoModels.CommentsDtos;
using BlogSystem.BLL.helpers;

namespace BlogSystem.BLL.Contracts
{
    public interface ICommentService
    {
        Task<CommentDto> AddComment(AddCommentDto comment, string authorId, int postId);
        Task<Pagination<CommentDto>> GetAllPostComments(int postId, int pageNumber);
        Task<CommentDto> UpdateComment(UpdateCommentDto comment, string authorId);
        Task DeleteComment(int commentId, string authorId);
    }
}
