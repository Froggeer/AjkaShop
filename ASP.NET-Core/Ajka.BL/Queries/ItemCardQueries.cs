using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.ItemCard;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.DAL.Model.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ajka.Common.Enums;
using Ajka.DAL.Model;

namespace Ajka.BL.Queries
{
    public class ItemCardQueries : IItemCardQueries, IAjkaShopService
    {
        private readonly IMapper _mapper;

        public ItemCardQueries(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ItemCard> GetEntityAsync(IAjkaShopDbContext ajkaShopDbContext, string commodityIdentifier, CancellationToken cancellationToken)
        {
            return await ajkaShopDbContext.ItemCard.AsNoTracking()
                .Where(x => x.CommodityIdentifier == commodityIdentifier)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ItemCardDto>> GetItemCardsAdministratorAsync(IAjkaShopDbContext ajkaShopDbContext, int categoryId, CancellationToken cancellationToken)
        {
            var itemCards = await ajkaShopDbContext.ItemCard.AsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .OrderBy(o => o.State)
                .ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ItemCardDto>>(itemCards);
        }

        public async Task<IEnumerable<ItemCardDto>> GetItemCardsAsync(IAjkaShopDbContext ajkaShopDbContext, string keyWord, CancellationToken cancellationToken)
        {
            var itemCards = await ajkaShopDbContext.ItemCard.AsNoTracking()
                .Where(x => (x.Headline.Contains(keyWord) || x.Description.Contains(keyWord)) && x.State == ItemCardEnums.ItemCardState.ForSale)
                .ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ItemCardDto>>(itemCards);
        }

        public async Task<IEnumerable<ItemCardDto>> GetItemCardsForSaleAsync(IAjkaShopDbContext ajkaShopDbContext, int categoryId, CancellationToken cancellationToken)
        {
            var itemCards = await ajkaShopDbContext.ItemCard.AsNoTracking()
                .Where(x => x.CategoryId == categoryId && x.State == ItemCardEnums.ItemCardState.ForSale)
                .ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ItemCardDto>>(itemCards);
        }
    }
}
