using Ajka.BL.Models.ProductImport;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Services.ItemCard.Interfaces
{
    public interface IItemCardService
    {
        Task UploadThumbnailImageAsync(int id, MemoryStream stream, string fileExtension, CancellationToken cancellationToken);

        Task SynchronizeAdlerItemCardsAsync(IEnumerable<ImportAdlerDto> itemCards, CancellationToken cancellationToken);
    }
}
