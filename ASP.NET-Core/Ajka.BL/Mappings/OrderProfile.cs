using Ajka.BL.Models.Order;
using AutoMapper;

namespace Ajka.BL.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<DAL.Model.Order, OrderDto>();
        }
    }
}
