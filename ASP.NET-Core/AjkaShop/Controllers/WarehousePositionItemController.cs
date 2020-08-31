using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Facades.Base;
using Ajka.BL.Models.Warehouse;
using Ajka.BL.Queries.Interfaces;
using Ajka.Common.Constants.Base;
using Ajka.DAL.Model.Interfaces;
using AjkaShop.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Goods stored in warehouse positions.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("warehouse-position-items")]
    public class WarehousePositionItemController : CrudController<WarehousePositionItemDto, int>
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IWarehousePositionItemQueries _warehousePositionItemQueries;

        public WarehousePositionItemController(IEntityDtoFacade<WarehousePositionItemDto, int> facade,
            IAjkaShopDbContext ajkaShopDbContext,
            IWarehousePositionItemQueries warehousePositionItemQueries) : base(facade)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _warehousePositionItemQueries = warehousePositionItemQueries;
        }

        /// <summary>
        /// Returns list of positions with detailed info of goods.
        /// </summary>
        /// <param name="cancellationToken"></param>
        [AllowAnonymous]
        [HttpGet, Route("overview"), SwaggerOperation(nameof(GetWarehousePositionItemsOverview))]
        public async Task<IEnumerable<WarehousePositionItemOverviewDto>> GetWarehousePositionItemsOverview(CancellationToken cancellationToken = default)
        {
            return await _warehousePositionItemQueries.GetWarehousePositionItemsOverviewAsync(_ajkaShopDbContext, cancellationToken).ConfigureAwait(false);
        }
    }
}
