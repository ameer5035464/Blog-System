using AutoMapper;
using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.DtoModels.CategoriesDtos;
using BlogSystem.BLL.GlobalExceptions.ExceptionModels;
using BlogSystem.DAL.Contracts;
using BlogSystem.DAL.Entities;

namespace BlogSystem.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            var mapCategories = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return mapCategories;
        }

        public async Task<CategoryDto> UpdateCategory(UpdateCategoryDto categoryUpdated)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetAsync(categoryUpdated.Id);

            if (category != null)
            {
                var mapCategory = _mapper.Map(categoryUpdated, category);

                _unitOfWork.GetRepository<Category>().Update(mapCategory);

                var mapDto = _mapper.Map<CategoryDto>(mapCategory);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.DisposeAsync();
                return mapDto;
            }
            throw new CustomBadRequest("No Category found with this data");
        }

        public async Task<CategoryDto> CreateCategory(CreateCategoryDto categoryCreated)
        {
            var category = _unitOfWork.GetRepository<Category>().GetAllAsync().Result.Where(C => C.Name == categoryCreated.Name).FirstOrDefault();

            if (category == null)
            {
                var mapCategory = _mapper.Map<Category>(categoryCreated);

                await _unitOfWork.GetRepository<Category>().AddAsync(mapCategory);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.DisposeAsync();

                var categoryDto = _mapper.Map<CategoryDto>(mapCategory);
                return categoryDto;
            }
            throw new CustomConflictException("This Category Already Exist");
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetAsync(categoryId);

            if (category != null)
            {
                _unitOfWork.GetRepository<Category>().Delete(category);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.DisposeAsync();
            }
            else
                throw new CustomBadRequest("no category was founded with this id to delete");
        }

    }
}
