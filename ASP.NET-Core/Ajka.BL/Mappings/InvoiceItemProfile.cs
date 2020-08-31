using Ajka.BL.Models.Invoice;
using Ajka.DAL.Model;
using AutoMapper;

namespace Ajka.BL.Mappings
{
    public class InvoiceItemProfile : Profile
    {
        public InvoiceItemProfile()
        {
            CreateMap<InvoiceItem, InvoiceItemDto>();
            CreateMap<InvoiceItemDto, InvoiceItem>();
        }
    }
}
