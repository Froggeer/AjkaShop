using Ajka.BL.Facades.Base;
using Ajka.BL.Models.User;
using Ajka.DAL;
using AutoMapper;

namespace Ajka.BL.Facades.User
{
    public class UserCrudFacade : RepositoryCrudFacade<UserDto, DAL.Model.User, int>, IEntityDtoFacade<UserDto, int>
    {
        public UserCrudFacade(AjkaShopDbContext ajkaShopDbContext, IMapper mapper) : base(ajkaShopDbContext, mapper)
        {
        }
    }
}
