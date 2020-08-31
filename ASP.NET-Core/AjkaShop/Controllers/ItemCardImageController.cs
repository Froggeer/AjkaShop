using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Exceptions;
using Ajka.BL.Models.ItemCard;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.ItemCard.Interfaces;
using Ajka.Common.Constants.Base;
using Ajka.DAL.Model.Interfaces;
using AjkaShop.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Images for item card.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("item-card-images")]
    public class ItemCardImageController : ControllerBase
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IItemCardImageService _itemCardImageService;
        private readonly IItemCardImageQueries _itemCardImageQueries;

        public ItemCardImageController(IAjkaShopDbContext ajkaShopDbContext,
            IItemCardImageService itemCardImageService,
            IItemCardImageQueries itemCardImageQueries)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _itemCardImageService = itemCardImageService;
            _itemCardImageQueries = itemCardImageQueries;
        }

        /// <summary>
        /// Returns collection of images, assigned to item card.
        /// </summary>
        /// <param name="itemCardId">Item card record Id</param>
        [AllowAnonymous]
        [HttpGet, Route("item-card-id/{itemCardId}"), SwaggerOperation(nameof(GetByItemCardId))]
        public async Task<IEnumerable<ItemCardImageDto>> GetByItemCardId([FromRoute] int itemCardId)
        {
            return await _itemCardImageQueries.GetItemCardImagesAsync(_ajkaShopDbContext, itemCardId, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Function for uploading item images.
        /// </summary>
        /// <param name="itemCardId">Image card record Id</param>
        /// <param name="form">Image file</param>
        [HttpPost, Route("{itemCardId}/upload-image"), SwaggerOperation(nameof(UploadImage))]
        public async Task UploadImage([FromRoute] int itemCardId, [FromForm] IFormCollection form)
        {
            if (!form.Files.Any())
            {
                throw new HttpResponseException
                {
                    Value = AjkaExceptions.E0004
                };
            }
            foreach (var file in form.Files)
            {
                var fileExtension = FilePropertiesCheck.CheckFileProperties(file);
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream).ConfigureAwait(false);
                await _itemCardImageService.UploadImageAsync(itemCardId, stream, fileExtension, CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Delete image and record.
        /// </summary>
        /// <param name="id">Item card image record Id</param>
        [HttpDelete, Route("{id}/delete-image"), SwaggerOperation(nameof(DeleteImage))]
        public async Task DeleteImage([FromRoute] int id)
        {
            await _itemCardImageService.DeleteImageAsync(id, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
