using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL.DtoModels.CommentsDtos
{
    public class UpdateCommentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Comment can't be empty")]
        public string Content { get; set; } = null!;
    }
}
