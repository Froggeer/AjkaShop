using Ajka.BL.Models.Category;
using AutoMapper;

namespace Ajka.BL.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<DAL.Model.Category, CategoryDto>();
            CreateMap<CategoryDto, DAL.Model.Category>();
        }
    }
}
