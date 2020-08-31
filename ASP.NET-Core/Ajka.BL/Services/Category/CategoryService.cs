using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.BL.Services.Category.Interfaces;
using Ajka.DAL.Model.Interfaces;
using Arch.EntityFrameworkCore.UnitOfWork;

namespace Ajka.BL.Services.Category
{
    public class CategoryService : ICategoryService, IAjkaShopService
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryQueries _categoryQueries;

        public CategoryService(IAjkaShopDbContext ajkaShopDbContext,
                               IUnitOfWork unitOfWork,
                               ICategoryQueries categoryQueries)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _unitOfWork = unitOfWork;
            _categoryQueries = categoryQueries;
        }

        public async Task<bool> PrepareCategoryForDeleteAsync(int id, CancellationToken cancellationToken)
        {
            var isItemCardsAssigned = await _categoryQueries.IsItemCardAssignedAsync(_ajkaShopDbContext, id, cancellationToken).ConfigureAwait(false);
            if (isItemCardsAssigned)
            {
                var categoryRepository = _unitOfWork.GetRepository<DAL.Model.Category>();
                var categoryRecord = await categoryRepository.FindAsync(id).ConfigureAwait(false);
                if (categoryRecord == null)
                {
                    return false;
                }
                categoryRecord.IsValid = false;
                categoryRepository.Update(categoryRecord);
                await _unitOfWork.SaveChangesAsync();
                return false;
            }
            return true;
        }

        public async Task SynchronizeGroupCategoriesAsync(IEnumerable<string> categories, CancellationToken cancellationToken)
        {
            var categoryRepository = _unitOfWork.GetRepository<DAL.Model.Category>();
            foreach (var categoryName in categories)
            {
                var isCategoryInDb = await _categoryQueries.IsCategoryExistingAsync(_ajkaShopDbContext, categoryName, cancellationToken).ConfigureAwait(false);
                if (!isCategoryInDb)
                {
                    var categoryForInsert = new DAL.Model.Category
                    {
                        Description = categoryName,
                        GroupIdentifier = categoryName,
                        IsValid = true
                    };
                    await categoryRepository.InsertAsync(categoryForInsert).ConfigureAwait(false);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
