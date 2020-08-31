using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.BL.Services.Invoice.Interfaces;
using Ajka.DAL.Model.Interfaces;
using Microsoft.AspNetCore.Hosting;
using RazorLight;
using SelectPdf;

namespace Ajka.BL.Services.Invoice
{
    public class InvoiceService : IInvoiceService, IAjkaShopService
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IInvoiceQueries _invoiceQueries;

        public InvoiceService(IAjkaShopDbContext ajkaShopDbContext,
                              IWebHostEnvironment environment,
                              IInvoiceQueries invoiceQueries)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _environment = environment;
            _invoiceQueries = invoiceQueries;
        }

        public async Task<byte[]> GetInvoiceInPdfFormatAsync(int id, CancellationToken cancellationToken)
        {
            var invoiceDetail = await _invoiceQueries.GetInvoiceDetailAsync(_ajkaShopDbContext, id, cancellationToken).ConfigureAwait(false);
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(_environment.ContentRootPath)
                .UseMemoryCachingProvider()
                .Build();
            invoiceDetail.InvoiceItems = invoiceDetail.InvoiceItems.OrderBy(o => o.OrderNumber).ToList();
            var htmlContent = await engine.CompileRenderAsync("Views/InvoiceTemplate.cshtml", invoiceDetail).ConfigureAwait(false);

            var converter = new HtmlToPdf();
            return converter.ConvertHtmlString(htmlContent).Save();
        }
    }
}
