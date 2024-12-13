using BlogSystem.DAL.Entities.Enums;

namespace BlogSystem.BLL.DtoModels.BlogPostDtos
{
    public class PostDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; } = null;
        public string Status { get; set; } = null!;
        public IEnumerable<string>? Tags { get; set; } = new List<string>();
        public string Category { get; set; } = null!;
    }
}
