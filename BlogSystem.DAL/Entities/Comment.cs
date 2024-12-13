namespace BlogSystem.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int PostId { get; set; }
        public virtual BlogPost Post { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
    }
}
