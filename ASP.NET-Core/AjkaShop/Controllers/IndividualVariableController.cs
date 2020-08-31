using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Exceptions;
using Ajka.BL.Facades.Base;
using Ajka.BL.Models.Base;
using Ajka.BL.Queries.Interfaces;
using Ajka.Common.Constants.Base;
using Ajka.Common.Models.Base;
using Ajka.DAL.Model.Interfaces;
using AjkaShop.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Individual variables of all types.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("individual-variables")]
    public class IndividualVariableController : CrudController<IndividualVariableDto, int>
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IIndividualVariableQueries _individualVariableQueries;

        public IndividualVariableController(IEntityDtoFacade<IndividualVariableDto, int> facade,
            IAjkaShopDbContext ajkaShopDbContext,
            IIndividualVariableQueries individualVariableQueries) : base(facade)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _individualVariableQueries = individualVariableQueries;
        }

        [NonAction]
        public override Task<IndividualVariableDto> Get([FromRoute] int id)
        {
            throw new HttpResponseException
            {
                Value = AjkaExceptions.E0003
            };
        }

        [NonAction]
        public override Task<IEnumerable<IndividualVariableDto>> Filter([FromBody] CrudFilterModel filterData)
        {
            throw new HttpResponseException
            {
                Value = AjkaExceptions.E0003
            };
        }

        [NonAction]
        public override Task<bool> Delete([FromRoute] int id)
        {
            throw new HttpResponseException
            {
                Value = AjkaExceptions.E0003
            };
        }

        /// <summary>
        /// Returns record of variable, based on her key. 
        /// </summary>
        /// <param name="keyName">Key parameter</param>
        [AllowAnonymous]
        [HttpGet, Route("key-name/{keyName}"), SwaggerOperation(nameof(GetByKeyName))]
        public async Task<IndividualVariableDto> GetByKeyName([FromRoute] string keyName)
        {
            return await _individualVariableQueries.GetIndividualVariableAsync(_ajkaShopDbContext, keyName, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
