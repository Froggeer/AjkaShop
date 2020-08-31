using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.Category;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.BL.Queries.Interfaces
{
    public interface ICategoryQueries
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesValidAsync(IAjkaShopDbContext ajkaShopDbContext, CancellationToken cancellationToken);

        Task<IDictionary<string, int>> GetCategoriesCommodityDictionaryAsync(IAjkaShopDbContext ajkaShopDbContext, CancellationToken cancellationToken);

        Task<bool> IsItemCardAssignedAsync(IAjkaShopDbContext ajkaShopDbContext, int id, CancellationToken cancellationToken);

        Task<bool> IsCategoryExistingAsync(IAjkaShopDbContext ajkaShopDbContext, string categoryName, CancellationToken cancellationToken);
    }
}