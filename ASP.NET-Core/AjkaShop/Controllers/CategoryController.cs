using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Facades.Base;
using Ajka.BL.Models.Category;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Category.Interfaces;
using Ajka.Common.Constants.Base;
using Ajka.DAL.Model.Interfaces;
using AjkaShop.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Categories of goods.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("categories")]
    public class CategoryController : CrudController<CategoryDto, int>
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryQueries _categoryQueries;

        public CategoryController(IEntityDtoFacade<CategoryDto, int> facade,
            IAjkaShopDbContext ajkaShopDbContext,
            ICategoryService categoryService,
            ICategoryQueries categoryQueries) : base(facade)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _categoryService = categoryService;
            _categoryQueries = categoryQueries;
        }

        /// <summary>
        /// Returns a record by primary ID.
        /// </summary>
        /// <param name="id">Record Id</param>
        [AllowAnonymous]
        [HttpGet, Route("{id}"), SwaggerOperation(nameof(Get))]
        public override async Task<CategoryDto> Get(int id)
        {
            return await base.Get(id).ConfigureAwait(false);
        }

        /// <summary>
        /// When are no item cards assigned, record is deleted, otherwise is invalidated.
        /// </summary>
        /// <param name="id">Record Id</param>
        public override async Task<bool> Delete(int id)
        {
            var isCategoryAbleForDelete = await _categoryService.PrepareCategoryForDeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
            if (isCategoryAbleForDelete)
            {
                return await base.Delete(id);
            }
            return false;
        }

        /// <summary>
        /// Returns only valid category records.
        /// </summary>
        [AllowAnonymous]
        [HttpGet, Route("valid"), SwaggerOperation(nameof(GetCategoriesValid))]
        public async Task<IEnumerable<CategoryDto>> GetCategoriesValid(CancellationToken cancellationToken = default)
        {
            return await _categoryQueries.GetCategoriesValidAsync(_ajkaShopDbContext, cancellationToken).ConfigureAwait(false);
        }
    }
}
