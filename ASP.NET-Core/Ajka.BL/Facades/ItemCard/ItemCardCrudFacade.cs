using Ajka.BL.Facades.Base;
using Ajka.BL.Models.ItemCard;
using Ajka.DAL;
using AutoMapper;

namespace Ajka.BL.Facades.ItemCard
{
    public class ItemCardCrudFacade : RepositoryCrudFacade<ItemCardDto, DAL.Model.ItemCard, int>, IEntityDtoFacade<ItemCardDto, int>
    {
        public ItemCardCrudFacade(AjkaShopDbContext ajkaShopDbContext, IMapper mapper) : base(ajkaShopDbContext, mapper)
        {
        }
    }
}
