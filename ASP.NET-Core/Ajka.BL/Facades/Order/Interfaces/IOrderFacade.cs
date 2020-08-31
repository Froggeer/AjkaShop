using Ajka.BL.Models.Order;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Facades.Order.Interfaces
{
    public interface IOrderFacade
    {
        Task<string> CreateOrderAsync(OrderBasketDto basket, CancellationToken cancellationToken);
    }
}
