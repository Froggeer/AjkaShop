using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.Order;
using Ajka.BL.Facades.Order.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using static Ajka.Common.Enums.OrderEnums;
using Ajka.DAL.Model.Interfaces;
using Ajka.BL.Queries.Interfaces;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Customer orders.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ControllerName("orders")]
    public class OrderController : ControllerBase
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IOrderFacade _orderFacade;
        private readonly IOrderQueries _orderQueries;

        public OrderController(IAjkaShopDbContext ajkaShopDbContext,
            IOrderFacade orderFacade,
            IOrderQueries orderQueries)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _orderFacade = orderFacade;
            _orderQueries = orderQueries;
        }

        /// <summary>
        /// Evidence of goods in basket and process customer order with send emails.
        /// </summary>
        /// <param name="order">Content of basket</param>
        /// <param name="cancellationToken"></param>
        [AllowAnonymous]
        [HttpPost, Route("create"), SwaggerOperation(nameof(Create))]
        public async Task<string> Create([FromBody] OrderBasketDto order, CancellationToken cancellationToken = default)
        {
            return await _orderFacade.CreateOrderAsync(order, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns list of orders by theirs State attribute.
        /// </summary>
        /// <param name="state">OrderState</param>
        /// <param name="cancellationToken"></param>
        [HttpGet, Route("state/{state}"), SwaggerOperation(nameof(GetByState))]
        public async Task<IEnumerable<OrderDto>> GetByState([FromRoute] OrderState state, CancellationToken cancellationToken = default)
        {
            return await _orderQueries.GetOrdersAsync(_ajkaShopDbContext, state, cancellationToken).ConfigureAwait(false);
        }
    }
}
