using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Exceptions;
using Ajka.BL.Facades.Base;
using Ajka.BL.Models.Invoice;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Invoice.Interfaces;
using Ajka.Common.Constants.Base;
using Ajka.DAL.Model.Interfaces;
using AjkaShop.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Evidence of invoices.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("invoices")]
    public class InvoiceController : CrudController<InvoiceDto, int>
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoiceQueries _invoiceQueries;

        public InvoiceController(IEntityDtoFacade<InvoiceDto, int> facade,
            IAjkaShopDbContext ajkaShopDbContext,
            IInvoiceService invoiceService,
            IInvoiceQueries invoiceQueries) : base(facade)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _invoiceService = invoiceService;
            _invoiceQueries = invoiceQueries;
        }

        [NonAction]
        public override Task<InvoiceDto> Get([FromRoute] int id)
        {
            throw new HttpResponseException
            {
                Value = AjkaExceptions.E0003
            };
        }

        /// <summary>
        /// Returns chosen invoice with assigned items.
        /// </summary>
        /// <param name="id">Invoice record ID</param>
        [HttpGet, Route("{id}/detail"), SwaggerOperation(nameof(GetDetail))]
        public async Task<InvoiceDetailDto> GetDetail([FromRoute] int id)
        {
            return await _invoiceQueries.GetInvoiceDetailAsync(_ajkaShopDbContext, id, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Generate pdf file using data of chosen invoice.
        /// </summary>
        /// <param name="id">Invoice record ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns>PDF file</returns>
        [AllowAnonymous]
        [HttpGet, Route("{id}/pdf-export"), SwaggerOperation(nameof(PdfExport))]
        public async Task<FileResult> PdfExport([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var pdfContent = await _invoiceService.GetInvoiceInPdfFormatAsync(id, cancellationToken).ConfigureAwait(false);
            return new FileContentResult(pdfContent, "application/pdf")
            {
                FileDownloadName = $"{nameof(PdfExport)}.pdf"
            };
        }
    }
}
