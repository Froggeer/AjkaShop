using Ajka.BL.Models.ItemCard;
using AutoMapper;

namespace Ajka.BL.Mappings
{
    public class ItemCardSizePriceProfile : Profile
    {
        public ItemCardSizePriceProfile()
        {
            CreateMap<DAL.Model.ItemCardSizePrice, ItemCardSizePriceDto>();
            CreateMap<ItemCardSizePriceDto, DAL.Model.ItemCardSizePrice>();
        }
    }
}
