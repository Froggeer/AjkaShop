using Ajka.BL.Models.ItemCard;
using AutoMapper;

namespace Ajka.BL.Mappings
{
    public class ItemCardImageProfile : Profile
    {
        public ItemCardImageProfile()
        {
            CreateMap<DAL.Model.ItemCardImage, ItemCardImageDto>();
            CreateMap<ItemCardImageDto, DAL.Model.ItemCardImage>();
        }
    }
}
