using System.ComponentModel.DataAnnotations;

namespace BlogSystem.BLL.DtoModels.CategoriesDtos
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
