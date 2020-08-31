using Ajka.BL.Models.Invoice;
using Ajka.DAL.Model.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Queries.Interfaces
{
    public interface IInvoiceQueries
    {
        Task<InvoiceDetailDto> GetInvoiceDetailAsync(IAjkaShopDbContext ajkaShopDbContext, int id, CancellationToken cancellationToken);
    }
}
