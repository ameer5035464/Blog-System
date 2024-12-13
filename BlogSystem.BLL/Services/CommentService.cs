using AutoMapper;
using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.DtoModels.CommentsDtos;
using BlogSystem.BLL.GlobalExceptions.ExceptionModels;
using BlogSystem.BLL.helpers;
using BlogSystem.DAL.Contracts;
using BlogSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogSystem.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<CommentDto> AddComment(AddCommentDto comment, string authorId, int postId)
        {

            var post = await _unitOfWork.GetRepository<BlogPost>().GetAsync(postId) ?? throw new CustomBadRequest("Can't add comment to non exist post");

            var user = await _userManager.FindByIdAsync(authorId);

            var mapComment = _mapper.Map<Comment>(comment);
            mapComment.AuthorId = user!.Id;
            mapComment.CreatedAt = DateTime.UtcNow;
            mapComment.PostId = postId;

            await _unitOfWork.GetRepository<Comment>().AddAsync(mapComment);
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.DisposeAsync();

            var commentDto = _mapper.Map<CommentDto>(mapComment);
            commentDto.AuthorName = user.UserName!;
            return commentDto;
        }

        public async Task<Pagination<CommentDto>> GetAllPostComments(int postId, int pageNumber)
        {
            var post = await _unitOfWork.GetRepository<BlogPost>().GetAsync(postId) ?? throw new CustomBadRequest("No Post with this id");

            var comments = await _unitOfWork.GetRepository<Comment>().GetAllAsync();

            var filterComments = comments.Where(C => C.PostId == postId).Select(C => new CommentDto
            {
                AuthorName = _userManager.FindByIdAsync(C.AuthorId).Result!.UserName!,
                Content = C.Content,
                CreatedAt = C.CreatedAt,
                UpdatedAt = C.UpdatedAt
            });

            var paginatedResponse = Pagination<CommentDto>.GetPagination(filterComments.AsQueryable(), pageNumber, 5);

            return paginatedResponse;
        }

        public async Task<CommentDto> UpdateComment(UpdateCommentDto comment, string authorId)
        {
            var getComment = await _unitOfWork.GetRepository<Comment>().GetAsync(comment.Id) ?? throw new CustomBadRequest("No Comment with this id");

            if (getComment.AuthorId == authorId)
            {

                var user = await _userManager.FindByIdAsync(authorId);
                var mapComment = _mapper.Map(comment, getComment);
                mapComment.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.GetRepository<Comment>().Update(mapComment);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.DisposeAsync();

                var commentDto = _mapper.Map<CommentDto>(mapComment);
                commentDto.AuthorName = user!.UserName!;
                return commentDto;
            }
            throw new UnauthorizedException("you can't modify another user Comment");
        }

        public async Task DeleteComment(int commentId, string authorId)
        {
            var getComment = await _unitOfWork.GetRepository<Comment>().GetAsync(commentId) ?? throw new CustomBadRequest("No Comment with this id");

            var user = await _userManager.FindByIdAsync(authorId);

            if (getComment.AuthorId == authorId || await _userManager.IsInRoleAsync(user!, "Admin"))
            {
                _unitOfWork.GetRepository<Comment>().Delete(getComment);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.DisposeAsync();
            }
            else
            throw new UnauthorizedException("you are not allowed to delete this comment");
        }
    }
}
