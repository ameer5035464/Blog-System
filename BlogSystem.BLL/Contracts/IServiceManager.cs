namespace BlogSystem.BLL.Contracts
{
    public interface IServiceManager
    {
        IAccountService AccountService { get; }
        IBlogPostService BlogPostService { get; }
        ICategoryService CategoryService { get; }
        ICommentService CommentService { get; }
    }
}
