using Ajka.BL.Models.ProductImport;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Services.ItemCard.Interfaces
{
    public interface IItemCardImageService
    {
        Task UploadImageAsync(int itemCardId, MemoryStream stream, string fileExtension, CancellationToken cancellationToken);

        Task DeleteImageAsync(int id, CancellationToken cancellationToken);

        Task SynchronizeAdlerImagesAsync(int itemCardId, ImportAdlerDto itemCardImport, CancellationToken cancellationToken);
    }
}
