using Ajka.BL.Models.ItemCard;
using Ajka.DAL.Model;
using Ajka.DAL.Model.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Queries.Interfaces
{
    public interface IItemCardSizePriceQueries
    {
        Task<IEnumerable<ItemCardSizePrice>> GetEntitiesAsync(IAjkaShopDbContext ajkaShopDbContext, int itemCardId, CancellationToken cancellationToken);

        Task<IEnumerable<ItemCardSizePriceDto>> GetItemCardSizePricesAsync(IAjkaShopDbContext ajkaShopDbContext, int itemCardId, CancellationToken cancellationToken);
    }
}
