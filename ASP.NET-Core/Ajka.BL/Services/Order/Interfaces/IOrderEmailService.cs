using Ajka.BL.Models.Order;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Services.Order.Interfaces
{
    public interface IOrderEmailService
    {
        Task SendAsync(string customersEmail, string emailBody, bool isRequestedCopyOfOrder, CancellationToken cancellationToken);

        Task<string> RenderViewToStringAsync(IList<OrderBasketItemDto> itemCardsInBasket, string email, string orderNumber, CancellationToken cancellationToken);
    }
}
