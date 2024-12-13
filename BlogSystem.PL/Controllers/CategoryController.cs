using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.DtoModels.CategoriesDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CategoryController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _serviceManager.CategoryService.GetCategories();

            return Ok(categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateNewCategory(CreateCategoryDto createCategory)
        {
            var Category = await _serviceManager.CategoryService.CreateCategory(createCategory);

            return Ok(Category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategory)
        {
            var category = await _serviceManager.CategoryService.UpdateCategory(updateCategory);

            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            await _serviceManager.CategoryService.DeleteCategory(categoryId);

            return Ok(new { message = "deleted succefully"});
        }

    }
}
