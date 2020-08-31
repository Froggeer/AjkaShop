using Ajka.BL.Models.ItemCard;
using Ajka.DAL.Model.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Queries.Interfaces
{
    public interface IItemCardQueries
    {
        Task<IEnumerable<ItemCardDto>> GetItemCardsForSaleAsync(IAjkaShopDbContext ajkaShopDbContext, int categoryId, CancellationToken cancellationToken);

        Task<IEnumerable<ItemCardDto>> GetItemCardsAdministratorAsync(IAjkaShopDbContext ajkaShopDbContext, int categoryId, CancellationToken cancellationToken);

        Task<IEnumerable<ItemCardDto>> GetItemCardsAsync(IAjkaShopDbContext ajkaShopDbContext, string keyWord, CancellationToken cancellationToken);

        Task<DAL.Model.ItemCard> GetEntityAsync(IAjkaShopDbContext ajkaShopDbContext, string commodityIdentifier, CancellationToken cancellationToken);
    }
}
