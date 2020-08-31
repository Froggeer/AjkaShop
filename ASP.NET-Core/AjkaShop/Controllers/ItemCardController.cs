using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Exceptions;
using Ajka.BL.Facades.Base;
using Ajka.BL.Models.ItemCard;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.ItemCard.Interfaces;
using Ajka.Common.Constants.Base;
using Ajka.DAL.Model.Interfaces;
using AjkaShop.Controllers.Base;
using AjkaShop.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Preview details of goods.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("item-cards")]
    public class ItemCardController : CrudController<ItemCardDto, int>
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IItemCardService _itemCardService;
        private readonly IItemCardQueries _itemCardQueries;

        public ItemCardController(IEntityDtoFacade<ItemCardDto, int> facade,
            IAjkaShopDbContext ajkaShopDbContext,
            IItemCardService itemCardService,
            IItemCardQueries itemCardQueries) : base(facade)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _itemCardService = itemCardService;
            _itemCardQueries = itemCardQueries;
        }

        /// <summary>
        /// Returns a record by primary ID.
        /// </summary>
        /// <param name="id">ID of record</param>
        [AllowAnonymous]
        public override async Task<ItemCardDto> Get(int id)
        {
            return await base.Get(id).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns item cards assigned to chosen category available for sale.
        /// </summary>
        /// <param name="categoryId">Category record Id</param>
        /// <param name="cancellationToken"></param>
        [AllowAnonymous]
        [HttpGet, Route("category-id/{categoryId}/for-sale"), SwaggerOperation(nameof(GetItemCardsForSale))]
        public async Task<IEnumerable<ItemCardDto>> GetItemCardsForSale([FromRoute] int categoryId, CancellationToken cancellationToken = default)
        {
            return await _itemCardQueries.GetItemCardsForSaleAsync(_ajkaShopDbContext, categoryId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns all item cards assigned to chosen category, sorted by theirs state.
        /// </summary>
        /// <param name="categoryId">Category record Id</param>
        /// <param name="cancellationToken"></param>
        [AllowAnonymous]
        [HttpGet, Route("category-id/{categoryId}/administrator"), SwaggerOperation(nameof(GetItemCardsAdministratorView))]
        public async Task<IEnumerable<ItemCardDto>> GetItemCardsAdministratorView([FromRoute] int categoryId, CancellationToken cancellationToken = default)
        {
            return await _itemCardQueries.GetItemCardsAdministratorAsync(_ajkaShopDbContext, categoryId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns all items cards, which they contains chosen word in headline or description.
        /// </summary>
        /// <param name="keyWord">Word for search</param>
        /// <param name="cancellationToken"></param>
        [AllowAnonymous]
        [HttpGet, Route("key-word/{keyWord}/search"), SwaggerOperation(nameof(Search))]
        public async Task<IEnumerable<ItemCardDto>> Search([FromRoute] string keyWord, CancellationToken cancellationToken = default)
        {
            return await _itemCardQueries.GetItemCardsAsync(_ajkaShopDbContext, keyWord, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Function for uploading goods previewing image.
        /// </summary>
        /// <param name="id">Record Id</param>
        /// <param name="form">Image file</param>
        [HttpPost, Route("{id}/upload-thumbnail-image"), SwaggerOperation(nameof(UploadThumbnailImage))]
        public async Task UploadThumbnailImage([FromRoute] int id, [FromForm] IFormCollection form)
        {
            if (!form.Files.Any())
            {
                throw new HttpResponseException
                {
                    Value = AjkaExceptions.E0004
                };
            }
            var file = form.Files.FirstOrDefault();

            var fileExtension = FilePropertiesCheck.CheckFileProperties(file);
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream).ConfigureAwait(false);
            await _itemCardService.UploadThumbnailImageAsync(id, stream, fileExtension, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
