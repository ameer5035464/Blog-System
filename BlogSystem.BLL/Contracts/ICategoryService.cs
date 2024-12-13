using BlogSystem.BLL.DtoModels.CategoriesDtos;

namespace BlogSystem.BLL.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategories();
        Task<CategoryDto> UpdateCategory(UpdateCategoryDto categoryUpdated);
        Task<CategoryDto> CreateCategory(CreateCategoryDto categoryCreated);
        Task DeleteCategory(int categoryId);
    }
}
