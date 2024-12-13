using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.DtoModels.CommentsDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSystem.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CommentController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPost("AddComment/{postId}")]
        public async Task<IActionResult> AddNewComment([FromBody] AddCommentDto commentAdded, int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);

            var comment = await _serviceManager.CommentService.AddComment(commentAdded, userId!, postId);

            return Ok(comment);
        }

        [Authorize]
        [HttpGet("GetComments/{postId}")]
        public async Task<IActionResult> GetAllComments(int postId, int pageNumber)
        {
            var comments = await _serviceManager.CommentService.GetAllPostComments(postId, pageNumber);

            return Ok(comments);
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPut("UpdateComment")]
        public async Task<IActionResult> UpdateCurrentComment(UpdateCommentDto updateComment)
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);

            var comment = await _serviceManager.CommentService.UpdateComment(updateComment, userId!);

            return Ok(comment);
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteCurrentComment(int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);

             await _serviceManager.CommentService.DeleteComment(commentId, userId!);

            return Ok(new {message = "Comment deleted"});
        }
    }
}
