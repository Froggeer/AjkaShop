using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Services.Invoice.Interfaces
{
    public interface IInvoiceService
    {
        Task<byte[]> GetInvoiceInPdfFormatAsync(int id, CancellationToken cancellationToken);
    }
}
