using Ajka.BL.Facades.Base;
using Ajka.BL.Models.Category;
using Ajka.BL.Models.User;
using Ajka.DAL;
using AutoMapper;

namespace Ajka.BL.Facades.Category
{
    public class CategoryCrudFacade : RepositoryCrudFacade<CategoryDto, DAL.Model.Category, int>, IEntityDtoFacade<CategoryDto, int>
    {
        public CategoryCrudFacade(AjkaShopDbContext ajkaShopDbContext, IMapper mapper) : base(ajkaShopDbContext, mapper)
        {
        }
    }
}
