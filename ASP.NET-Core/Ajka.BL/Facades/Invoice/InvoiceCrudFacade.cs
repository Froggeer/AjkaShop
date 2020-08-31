using Ajka.BL.Facades.Base;
using Ajka.BL.Models.Invoice;
using Ajka.DAL;
using AutoMapper;

namespace Ajka.BL.Facades.Invoice
{
    public class InvoiceCrudFacade : RepositoryCrudFacade<InvoiceDto, DAL.Model.Invoice, int>, IEntityDtoFacade<InvoiceDto, int>
    {
        public InvoiceCrudFacade(AjkaShopDbContext ajkaShopDbContext, IMapper mapper) : base(ajkaShopDbContext, mapper)
        {
        }
    }
}
