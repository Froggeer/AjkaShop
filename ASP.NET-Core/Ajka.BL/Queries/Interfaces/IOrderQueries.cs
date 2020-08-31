using Ajka.BL.Models.Order;
using Ajka.DAL.Model.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Ajka.Common.Enums.OrderEnums;

namespace Ajka.BL.Queries.Interfaces
{
    public interface IOrderQueries
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync(IAjkaShopDbContext ajkaShopDbContext, OrderState state, CancellationToken cancellationToken);
    }
}
