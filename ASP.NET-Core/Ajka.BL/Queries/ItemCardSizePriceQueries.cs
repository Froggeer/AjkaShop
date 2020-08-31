using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.ItemCard;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.DAL.Model;
using Ajka.DAL.Model.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ajka.BL.Queries
{
    public class ItemCardSizePriceQueries : IItemCardSizePriceQueries, IAjkaShopService
    {
        private readonly IMapper _mapper;

        public ItemCardSizePriceQueries(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemCardSizePrice>> GetEntitiesAsync(IAjkaShopDbContext ajkaShopDbContext, int itemCardId, CancellationToken cancellationToken)
        {
            return await ajkaShopDbContext.ItemCardSizePrice.AsNoTracking()
                .Where(x => x.ItemCardId == itemCardId)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ItemCardSizePriceDto>> GetItemCardSizePricesAsync(IAjkaShopDbContext ajkaShopDbContext, int itemCardId, CancellationToken cancellationToken)
        {
            var itemCardImages = await ajkaShopDbContext.ItemCardSizePrice.AsNoTracking()
                .Where(x => x.ItemCardId == itemCardId)
                .OrderBy(o => o.Id)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ItemCardSizePriceDto>>(itemCardImages);
        }
    }
}
