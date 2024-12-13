using AutoMapper;
using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.helpers;
using BlogSystem.DAL.Contracts;
using BlogSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BlogSystem.BLL.Services
{
    internal class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<IBlogPostService> _blogPostService;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<ICommentService> _commentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPhotoService _photoService;
        private readonly IOptions<JwtSettings> _jwtoptions;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceManager(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPhotoService photoService,
            IOptions<JwtSettings> Jwtoptions,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _photoService = photoService;
            _jwtoptions = Jwtoptions;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = new Lazy<IAccountService>(new AccountService(_userManager, _signInManager, photoService, _jwtoptions , _unitOfWork));
            _blogPostService = new Lazy<IBlogPostService>(new BlogPostService(_unitOfWork, _mapper, _userManager));
            _categoryService = new Lazy<ICategoryService>(new CategoryService(_unitOfWork, _mapper));
            _commentService = new Lazy<ICommentService>(new CommentService(_unitOfWork, _mapper, _userManager));
        }

        public IAccountService AccountService => _accountService.Value;
        public IBlogPostService BlogPostService => _blogPostService.Value;
        public ICategoryService CategoryService => _categoryService.Value;
        public ICommentService CommentService => _commentService.Value;
    }
}
