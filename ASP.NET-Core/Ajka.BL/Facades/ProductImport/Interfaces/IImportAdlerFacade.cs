using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Facades.ProductImport.Interfaces
{
    public interface IImportAdlerFacade
    {
        Task ImportAsync(string productsData, CancellationToken cancellationToken);
    }
}
