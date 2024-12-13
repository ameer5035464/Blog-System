using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.DtoModels.BlogPostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSystem.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public PostController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePost(CreatePostDto createPost)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.PrimarySid);

            var PostDto = await _serviceManager.BlogPostService.CreatePost(createPost, currentUser!);

            return Ok(PostDto);
        }

        [HttpGet("PublishedPosts")]
        public async Task<IActionResult> GetUserPublishedPosts(int categoryId, int pageNumber)
        {
            var posts = await _serviceManager.BlogPostService.GetPublishedPosts(categoryId, pageNumber);

            return Ok(posts);
        }

        [HttpGet("UserPublishedPosts/{userId}")]
        public async Task<IActionResult> GetUserPublishedPosts(int categoryId, int pageNumber, string userId)
        {
            var posts = await _serviceManager.BlogPostService.GetCurrentPublishedUserPosts(userId, categoryId, pageNumber);

            return Ok(posts);
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpGet("UserDraftPosts")]
        public async Task<IActionResult> GetDraftPosts(int categoryId, int pageNumber)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.PrimarySid);

            var posts = await _serviceManager.BlogPostService.GetCurrentDraftsUserPosts(currentUser!, categoryId, pageNumber);

            return Ok(posts);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ArchivedPosts/{AuthorId}")]
        public async Task<IActionResult> GetArchivedPosts(string AuthorId, int categoryId, int pageNumber)
        {
            var posts = await _serviceManager.BlogPostService.GetCurrentArchivedPosts(AuthorId, categoryId, pageNumber);

            return Ok(posts);
        }

        [Authorize]
        [HttpGet("GetPost/{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.PrimarySid);

            var posts = await _serviceManager.BlogPostService.GetPostById(currentUser!, postId);

            return Ok(posts);
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPost("UpdatePost")]
        public async Task<IActionResult> UpdateUserPost(UpdatePostDto updatePostDto)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.PrimarySid);
            var post = await _serviceManager.BlogPostService.UpdatePost(updatePostDto, currentUser!);

            return Ok(post);
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpDelete("DeletePost")]
        public async Task<IActionResult> DeleteUserPost(int postId)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.PrimarySid)!;
            await _serviceManager.BlogPostService.DeletePost(postId, currentUser);

            return Ok(new { message = "Deleted Succefully" });
        }
    }
}
