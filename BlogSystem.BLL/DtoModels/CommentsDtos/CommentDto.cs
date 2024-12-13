namespace BlogSystem.BLL.DtoModels.CommentsDtos
{
    public class CommentDto
    {
        public string Content { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string AuthorName { get; set; } = null!;
    }
}
