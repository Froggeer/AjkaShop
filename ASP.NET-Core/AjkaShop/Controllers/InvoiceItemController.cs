using Ajka.BL.Exceptions;
using Ajka.BL.Facades.Base;
using Ajka.BL.Models.Invoice;
using Ajka.Common.Constants.Base;
using Ajka.Common.Models.Base;
using AjkaShop.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Evidence of invoice items.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("invoice-items")]
    public class InvoiceItemController : CrudController<InvoiceItemDto, int>
    {
        public InvoiceItemController(IEntityDtoFacade<InvoiceItemDto, int> facade) : base(facade)
        {
        }

        [NonAction]
        public override Task<InvoiceItemDto> Get([FromRoute] int id)
        {
            throw new HttpResponseException
            {
                Value = AjkaExceptions.E0003
            };
        }

        [NonAction]
        public override Task<IEnumerable<InvoiceItemDto>> Filter([FromBody] CrudFilterModel filterData)
        {
            throw new HttpResponseException
            {
                Value = AjkaExceptions.E0003
            };
        }
    }
}
