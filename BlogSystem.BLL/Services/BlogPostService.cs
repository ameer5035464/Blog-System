using AutoMapper;
using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.DtoModels.BlogPostDtos;
using BlogSystem.BLL.GlobalExceptions.ExceptionModels;
using BlogSystem.BLL.helpers;
using BlogSystem.DAL.Contracts;
using BlogSystem.DAL.Entities;
using BlogSystem.DAL.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace BlogSystem.BLL.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogPostService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PostDto> CreatePost(CreatePostDto postDto, string currentUser)
        {
            var user = await _userManager.FindByIdAsync(currentUser);
            postDto.AuthorId = user!.Id;
            var category = await _unitOfWork.GetRepository<Category>().GetAsync(postDto.CategoryId) ?? throw new CustomBadRequest("No Category with this id");

            var allTags = await GetPostTags(postDto.Content);

            var mapblogPost = _mapper.Map<BlogPost>(postDto);

            await _unitOfWork.GetRepository<BlogPost>().AddAsync(mapblogPost);

            await _unitOfWork.CompleteAsync();


            var blogPostTags = allTags.Select(tag => new BlogPostTag
            {
                BlogPostId = mapblogPost.Id,
                TagId = tag.Id
            }).ToList();

            foreach (var item in blogPostTags)
            {
                await _unitOfWork.GetRepository<BlogPostTag>().AddAsync(item);
            }

            await _unitOfWork.CompleteAsync();
            await _unitOfWork.DisposeAsync();

            var postDtotoReturn = new PostDto
            {
                AuthorName = user!.UserName!,
                Category = category.Name,
                Content = mapblogPost.Content,
                CreatedAt = mapblogPost.CreatedAt,
                UpdatedAt = mapblogPost.UpdatedAt,
                Status = mapblogPost.Status.ToString(),
                Tags = allTags.Select(T => T.Name),
                Title = postDto.Title
            };

            return postDtotoReturn;
        }

        public async Task<Pagination<PostDto>> GetPublishedPosts(int categoryId, int pageNumber)
        {
            var posts = await _unitOfWork.GetRepository<BlogPost>().GetAllAsync();
            var postTag = await _unitOfWork.GetRepository<BlogPostTag>().GetAllAsync();

            var postToMap = categoryId == 0 ? posts.Where(P => P.Status == Status.Published)
                                         : posts.Where(P => P.Status == Status.Published && P.CategoryId == categoryId);

            var mapPosts = postToMap.Select(o => new PostDto()
            {
                AuthorName = _userManager.FindByIdAsync(o.AuthorId).Result!.UserName!,
                Content = o.Content,
                Title = o.Title,
                CreatedAt = o.CreatedAt,
                Status = o.Status.ToString(),
                UpdatedAt = o.UpdatedAt,
                Category = o.Category.Name,
                Tags = postTag.Where(pt => pt.BlogPostId == o.Id).Select(o => o.Tag.Name)
            });

            var afterPageing = Pagination<PostDto>.GetPagination(mapPosts.AsQueryable(), pageNumber, 5);

            return afterPageing;
        }

        public async Task<Pagination<PostDto>> GetCurrentPublishedUserPosts(string userId, int categoryId, int pageNumber)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var posts = await _unitOfWork.GetRepository<BlogPost>().GetAllAsync();
            var postTag = await _unitOfWork.GetRepository<BlogPostTag>().GetAllAsync();

            var postToMap = categoryId == 0 ? posts.Where(P => user!.Id == P.AuthorId && P.Status == Status.Published)
                                         : posts.Where(P => user!.Id == P.AuthorId && P.Status == Status.Published && P.CategoryId == categoryId);

            var mapPosts = postToMap.Select(o => new PostDto()
            {
                AuthorName = user!.UserName!,
                Content = o.Content,
                Title = o.Title,
                CreatedAt = o.CreatedAt,
                Status = o.Status.ToString(),
                UpdatedAt = o.UpdatedAt,
                Category = o.Category.Name,
                Tags = postTag.Where(pt => pt.BlogPostId == o.Id).Select(o => o.Tag.Name)
            });

            var afterPageing = Pagination<PostDto>.GetPagination(mapPosts.AsQueryable(), pageNumber, 5);

            return afterPageing;
        }

        public async Task<Pagination<PostDto>> GetCurrentDraftsUserPosts(string currentUser, int categoryId, int pageNumber)
        {
            var user = await _userManager.FindByIdAsync(currentUser);
            var posts = await _unitOfWork.GetRepository<BlogPost>().GetAllAsync();
            var postTag = await _unitOfWork.GetRepository<BlogPostTag>().GetAllAsync();

            var postToMap = categoryId == 0 ? posts.Where(P => user!.Id == P.AuthorId && P.Status == Status.Draft)
                                         : posts.Where(P => user!.Id == P.AuthorId && P.Status == Status.Draft && P.CategoryId == categoryId);

            var mapPosts = postToMap.Select(o => new PostDto()
            {
                AuthorName = user!.UserName!,
                Content = o.Content,
                Title = o.Title,
                CreatedAt = o.CreatedAt,
                Status = o.Status.ToString(),
                UpdatedAt = o.UpdatedAt,
                Category = o.Category.Name,
                Tags = postTag.Where(pt => pt.BlogPostId == o.Id).Select(o => o.Tag.Name)
            });

            var afterPageing = Pagination<PostDto>.GetPagination(mapPosts.AsQueryable(), pageNumber, 5);

            return afterPageing;
        }

        public async Task<Pagination<PostDto>> GetCurrentArchivedPosts(string authorId, int categoryId, int pageNumber)
        {
            var posts = await _unitOfWork.GetRepository<BlogPost>().GetAllAsync();
            var postTag = await _unitOfWork.GetRepository<BlogPostTag>().GetAllAsync();

            var postToMap = categoryId == 0 ? posts.Where(P => authorId == P.AuthorId && P.Status == Status.Archived)
                                            : posts.Where(P => authorId == P.AuthorId && P.Status == Status.Archived && P.CategoryId == categoryId);

            var mapPosts = postToMap.Select(o => new PostDto()
            {
                AuthorName = authorId,
                Content = o.Content,
                Title = o.Title,
                CreatedAt = o.CreatedAt,
                Status = o.Status.ToString(),
                UpdatedAt = o.UpdatedAt,
                Category = o.Category.Name,
                Tags = postTag.Where(pt => pt.BlogPostId == o.Id).Select(o => o.Tag.Name)
            });

            var afterPageing = Pagination<PostDto>.GetPagination(mapPosts.AsQueryable(), pageNumber, 5);

            return afterPageing;
        }

        public async Task<PostDto> GetPostById(string currentUser, int postId)
        {
            var post = await _unitOfWork.GetRepository<BlogPost>().GetAsync(postId);

            var BlogPostTags = await _unitOfWork.GetRepository<BlogPostTag>().GetAllAsync();

            var BlogPostTagsFilter = BlogPostTags.Where(BT => BT.BlogPostId == post!.Id).Select(BT => new Tag
            {
                Id = BT.TagId
            });

            var Tagsss = await _unitOfWork.GetRepository<Tag>().GetAllAsync();

            var finalTag = new List<string>();

            foreach (var tag in Tagsss)
            {
                if (BlogPostTagsFilter.Any(T => T.Id == tag.Id))
                {
                    finalTag.Add(tag.Name);
                }
            }

            if (post != null)
            {
                var user = await _userManager.FindByIdAsync(currentUser);

                var mapPost = new PostDto
                {
                    AuthorName = user!.UserName!,
                    Content = post.Content,
                    Title = post.Title,
                    CreatedAt = post.CreatedAt,
                    Status = post.Status.ToString(),
                    UpdatedAt = post.UpdatedAt,
                    Category = post.Category.Name,
                    Tags = finalTag
                };

                return mapPost;
            }

            throw new CustomBadRequest("No post with this id");
        }

        public async Task<PostDto> UpdatePost(UpdatePostDto updatePost, string currentUser)
        {
            var blogPost = await _unitOfWork.GetRepository<BlogPost>().GetAsync(updatePost.Id) ?? throw new CustomBadRequest("No Post with this Id");
            var category = await _unitOfWork.GetRepository<Category>().GetAsync(updatePost.CategoryId) ?? throw new CustomBadRequest("No Category with this id");
            var blogPostGetAll = await _unitOfWork.GetRepository<BlogPostTag>().GetAllAsync();
            var user = await _userManager.FindByIdAsync(currentUser);
            var allTags = await GetPostTags(updatePost.Content);
            var PostTag = allTags.Select(tag => new BlogPostTag
            {
                TagId = tag.Id,
                BlogPostId = blogPost.Id
            }).ToList();

            updatePost.AuthorId = user!.Id;

            _mapper.Map(updatePost, blogPost);
            blogPost.UpdatedAt = DateTimeOffset.Now;

            if (user.Id == blogPost.AuthorId)
            {
                _unitOfWork.GetRepository<BlogPost>().Update(blogPost);

                foreach (var item in blogPostGetAll)
                {
                    if (item.BlogPostId == blogPost.Id)
                        _unitOfWork.GetRepository<BlogPostTag>().Delete(item);
                }

                foreach (var item in PostTag)
                    await _unitOfWork.GetRepository<BlogPostTag>().AddAsync(item);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.DisposeAsync();

                var postDtoToReturn = new PostDto
                {
                    AuthorName = user!.UserName!,
                    Category = category.Name,
                    Content = blogPost.Content,
                    CreatedAt = blogPost.CreatedAt,
                    UpdatedAt = blogPost.UpdatedAt,
                    Status = blogPost.Status.ToString(),
                    Tags = allTags.Select(T => T.Name),
                    Title = blogPost.Title
                };

                return postDtoToReturn;
            }
            throw new UnauthorizedException("you are not authorized to modify this post");
        }

        public async Task DeletePost(int postId, string currentUser)
        {
            var post = await _unitOfWork.GetRepository<BlogPost>().GetAsync(postId);
            var user = await _userManager.FindByIdAsync(currentUser);

            if (post != null)
            {
                if (user!.Id == post.AuthorId || await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    _unitOfWork.GetRepository<BlogPost>().Delete(post);
                    await _unitOfWork.CompleteAsync();
                    await _unitOfWork.DisposeAsync();
                }
                else
                    throw new UnauthorizedException("you are not authorized to delete this post");
            }
            else
                throw new CustomBadRequest("no post found with this id");
        }


        #region helpers
        private async Task<List<Tag>> GetPostTags(string content)
        {
            var tags = content.Split(' ')
                      .Where(word => word.StartsWith("#"))
                      .Select(word => word.TrimStart('#').ToLower())
                      .Distinct()
                      .ToList();

            var tagCheck = await _unitOfWork.GetRepository<Tag>().GetAllAsync();

            var existingTags = tagCheck.Where(t => tags.Contains(t.Name)).ToList();

            var newTags = tags.Except(existingTags.Select(t => t.Name)).ToList();

            foreach (var newTag in newTags)
            {
                var tagEntity = new Tag { Name = newTag };
                await _unitOfWork.GetRepository<Tag>().AddAsync(tagEntity);
                await _unitOfWork.CompleteAsync();
                existingTags.Add(tagEntity);
            }
            return existingTags;
        }

        #endregion

    }
}
