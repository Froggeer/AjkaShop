using Ajka.BL.Facades.Base;
using Ajka.BL.Models.Warehouse;
using Ajka.Common.Constants.Base;
using AjkaShop.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Positions in warehouse.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("warehouse-positions")]
    public class WarehousePositionController : CrudController<WarehousePositionDto, int>
    {
        public WarehousePositionController(IEntityDtoFacade<WarehousePositionDto, int> facade) : base(facade)
        {
        }
    }
}
