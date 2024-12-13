using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL.DtoModels.BlogPostDtos
{
    public class CreatePostDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
        public int CategoryId { get; set; } 
        public string Status { get; set; } = null!;
    }
}
