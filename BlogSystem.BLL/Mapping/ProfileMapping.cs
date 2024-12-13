using AutoMapper;
using BlogSystem.BLL.DtoModels.BlogPostDtos;
using BlogSystem.BLL.DtoModels.CategoriesDtos;
using BlogSystem.BLL.DtoModels.CommentsDtos;
using BlogSystem.BLL.DtoModels.TagDto;
using BlogSystem.DAL.Entities;

namespace BlogSystem.BLL.Mapping
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            CreateMap<TagDto, Tag>();
            CreateMap<Category, CategoryDto>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<CreatePostDto, BlogPost>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdatePostDto, BlogPost>()
                 .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<AddCommentDto, Comment>();
            CreateMap<Comment, CommentDto>();
            CreateMap<UpdateCommentDto, Comment>();


        }
    }
}
