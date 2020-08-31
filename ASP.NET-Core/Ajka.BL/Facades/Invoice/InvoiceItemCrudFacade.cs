using Ajka.BL.Facades.Base;
using Ajka.BL.Models.Invoice;
using Ajka.DAL;
using AutoMapper;

namespace Ajka.BL.Facades.Invoice
{
    public class InvoiceItemCrudFacade : RepositoryCrudFacade<InvoiceItemDto, DAL.Model.InvoiceItem, int>, IEntityDtoFacade<InvoiceItemDto, int>
    {
        public InvoiceItemCrudFacade(AjkaShopDbContext ajkaShopDbContext, IMapper mapper) : base(ajkaShopDbContext, mapper)
        {
        }
    }
}
