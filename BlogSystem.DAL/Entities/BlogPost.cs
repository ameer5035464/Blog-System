using BlogSystem.DAL.Entities.Enums;

namespace BlogSystem.DAL.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public required string AuthorId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public Status Status { get; set; } = Status.Published;
        public virtual ICollection<BlogPostTag>? BlogPostTags { get; set; } = new List<BlogPostTag>();
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
    }
}
