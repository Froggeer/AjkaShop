using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Exceptions;
using Ajka.BL.Facades.ProductImport.Interfaces;
using Ajka.Common.Constants.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Imports from external sources.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("product-imports")]
    public class ProductImportController : ControllerBase
    {
        private readonly IImportAdlerFacade _importAdlerFacade;

        public ProductImportController(IImportAdlerFacade importAdlerFacade)
        {
            _importAdlerFacade = importAdlerFacade;
        }

        /// <summary>
        /// Converts data of products from xml file, obtained from Adler Point service.
        /// </summary>
        /// <param name="form">XML file</param>
        /// <param name="cancellationToken"></param>
        //[AllowAnonymous] // TODO for test only
        [HttpPost, Route("import-adler-product-list"), SwaggerOperation(nameof(ImportAdlerProductList))]
        public async Task ImportAdlerProductList([FromForm] IFormCollection form, CancellationToken cancellationToken = default)
        {
            if (!form.Files.Any())
            {
                throw new HttpResponseException
                {
                    Value = AjkaExceptions.E0004
                };
            }
            var file = form.Files.FirstOrDefault();
            if (file.ContentType.Equals("application/xml") || file.ContentType.Equals("text/xml"))
            {
                var result = new StringBuilder();
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        result.AppendLine(reader.ReadLine());
                    }
                }
                await _importAdlerFacade.ImportAsync(result.ToString(), cancellationToken).ConfigureAwait(false);
                return;
            }
            throw new HttpResponseException
            {
                Value = AjkaExceptions.E0006
            };
        }
    }
}
