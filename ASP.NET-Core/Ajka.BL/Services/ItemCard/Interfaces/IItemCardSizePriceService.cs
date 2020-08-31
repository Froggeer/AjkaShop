using Ajka.BL.Models.ProductImport;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Services.ItemCard.Interfaces
{
    public interface IItemCardSizePriceService
    {
        Task SynchronizeSizePriceVariantsAsync(int itemCardId, IList<ImportAdlerSizeDto> importSizePrices, CancellationToken cancellationToken);
    }
}
