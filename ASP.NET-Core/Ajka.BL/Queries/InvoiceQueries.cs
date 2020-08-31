using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.Invoice;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.DAL.Model.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ajka.BL.Queries
{
    public class InvoiceQueries : IInvoiceQueries, IAjkaShopService
    {
        private readonly IMapper _mapper;

        public InvoiceQueries(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<InvoiceDetailDto> GetInvoiceDetailAsync(IAjkaShopDbContext ajkaShopDbContext, int id, CancellationToken cancellationToken)
        {
            var invoice = await ajkaShopDbContext.Invoice.AsNoTracking()
                .Where(x => x.Id == id)
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            return _mapper.Map<InvoiceDetailDto>(invoice);
        }
    }
}
