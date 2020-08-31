using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.Category;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.DAL.Model.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ajka.BL.Queries
{
    public class CategoryQueries : ICategoryQueries, IAjkaShopService
    {
        private readonly IMapper _mapper;

        public CategoryQueries(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IDictionary<string, int>> GetCategoriesCommodityDictionaryAsync(IAjkaShopDbContext ajkaShopDbContext, CancellationToken cancellationToken)
        {
            return await ajkaShopDbContext.Category.AsNoTracking()
                .Where(x => x.GroupIdentifier != null)
                .ToDictionaryAsync(key => key.GroupIdentifier, value => value.Id, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesValidAsync(IAjkaShopDbContext ajkaShopDbContext, CancellationToken cancellationToken)
        {
            var userRecord = await ajkaShopDbContext.Category.AsNoTracking()
                .Where(x => x.IsValid)
                .OrderBy(o => o.Description)
                .ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<CategoryDto>>(userRecord);
        }

        public async Task<bool> IsCategoryExistingAsync(IAjkaShopDbContext ajkaShopDbContext, string categoryName, CancellationToken cancellationToken)
        {
            return await ajkaShopDbContext.Category.AsNoTracking()
                .AnyAsync(x => x.GroupIdentifier == categoryName, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> IsItemCardAssignedAsync(IAjkaShopDbContext ajkaShopDbContext, int id, CancellationToken cancellationToken)
        {
            return await ajkaShopDbContext.Category.AsNoTracking()
                .AnyAsync(x => x.Id == id && x.ItemCards.Any(), cancellationToken).ConfigureAwait(false);
        }
    }
}
