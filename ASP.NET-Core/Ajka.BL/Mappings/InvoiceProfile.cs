using Ajka.BL.Models.Invoice;
using Ajka.DAL.Model;
using AutoMapper;

namespace Ajka.BL.Mappings
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<InvoiceDto, Invoice>();

            CreateMap<Invoice, InvoiceDetailDto>();
        }
    }
}
