using Ajka.BL.Models.ItemCard;
using AutoMapper;

namespace Ajka.BL.Mappings
{
    public class ItemCardProfile : Profile
    {
        public ItemCardProfile()
        {
            CreateMap<DAL.Model.ItemCard, ItemCardDto>();
            CreateMap<ItemCardDto, DAL.Model.ItemCard>();
        }
    }
}
