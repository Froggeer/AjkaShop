using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Services.Category.Interfaces
{
    public interface ICategoryService
    {
        Task<bool> PrepareCategoryForDeleteAsync(int id, CancellationToken cancellationToken);

        Task SynchronizeGroupCategoriesAsync(IEnumerable<string> categories, CancellationToken cancellationToken);
    }
}
