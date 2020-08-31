using Ajka.BL.Models.ItemCard;
using Ajka.BL.Models.Order;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Services.Order.Interfaces
{
    public interface IOrderItemService
    {
        Task<IList<OrderBasketItemDto>> CreateOrderItemsFromBasketAsync(IList<OrderBasketItemDto> basketItems, int orderId, CancellationToken cancellationToken);
    }
}
