using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.ItemCard;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.ItemCard.Interfaces;
using Ajka.DAL.Model.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Size and price variations for item card.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ControllerName("item-card-size-prices")]
    public class ItemCardSizePriceController : ControllerBase
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IItemCardSizePriceService _itemCardSizePriceService;
        private readonly IItemCardSizePriceQueries _itemCardSizePriceQueries;

        public ItemCardSizePriceController(IAjkaShopDbContext ajkaShopDbContext,
            IItemCardSizePriceService itemCardSizePriceService,
            IItemCardSizePriceQueries itemCardSizePriceQueries)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _itemCardSizePriceService = itemCardSizePriceService;
            _itemCardSizePriceQueries = itemCardSizePriceQueries;
        }

        /// <summary>
        /// Returns collection of size and price variations, assigned to item card.
        /// </summary>
        /// <param name="itemCardId">Item card record Id</param>
        [AllowAnonymous]
        [HttpGet, Route("item-card-id/{itemCardId}"), SwaggerOperation(nameof(GetByItemCardId))]
        public async Task<IEnumerable<ItemCardSizePriceDto>> GetByItemCardId([FromRoute] int itemCardId)
        {
            return await _itemCardSizePriceQueries.GetItemCardSizePricesAsync(_ajkaShopDbContext, itemCardId, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
