using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.Order;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.DAL.Model.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static Ajka.Common.Enums.OrderEnums;

namespace Ajka.BL.Queries
{
    public class OrderQueries : IOrderQueries, IAjkaShopService
    {
        private readonly IMapper _mapper;

        public OrderQueries(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(IAjkaShopDbContext ajkaShopDbContext, OrderState state, CancellationToken cancellationToken)
        {
            var itemCards = await ajkaShopDbContext.Order.AsNoTracking()
                .Where(x => x.State == state)
                .OrderBy(o => o.CreateDate)
                .ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<OrderDto>>(itemCards);
        }
    }
}
