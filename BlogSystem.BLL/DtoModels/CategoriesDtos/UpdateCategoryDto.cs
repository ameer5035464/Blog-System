using System.ComponentModel.DataAnnotations;

namespace BlogSystem.BLL.DtoModels.CategoriesDtos
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
