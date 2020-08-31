using Ajka.BL.Models.User;
using AutoMapper;

namespace Ajka.BL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DAL.Model.User, UserDto>();
            CreateMap<UserDto, DAL.Model.User>();
        }
    }
}
