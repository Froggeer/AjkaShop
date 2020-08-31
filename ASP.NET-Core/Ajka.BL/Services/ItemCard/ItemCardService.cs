using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.ProductImport;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.BL.Services.ItemCard.Interfaces;
using Ajka.Common.Constants.Service;
using Ajka.Common.Helpers;
using Ajka.DAL.Model.Interfaces;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Hosting;

namespace Ajka.BL.Services.ItemCard
{
    public class ItemCardService : IItemCardService, IAjkaShopService
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IItemCardImageService _itemCardImageService;
        private readonly IItemCardSizePriceService _itemCardSizePriceService;
        private readonly IItemCardQueries _itemCardQueries;
        private readonly ICategoryQueries _categoryQueries;

        public ItemCardService(IAjkaShopDbContext ajkaShopDbContext,
                               IUnitOfWork unitOfWork,
                               IWebHostEnvironment environment,
                               IFileProcessingService fileProcessingService,
                               IItemCardImageService itemCardImageService,
                               IItemCardSizePriceService itemCardSizePriceService,
                               IItemCardQueries itemCardQueries,
                               ICategoryQueries categoryQueries)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _unitOfWork = unitOfWork;
            _environment = environment;
            _fileProcessingService = fileProcessingService;
            _itemCardImageService = itemCardImageService;
            _itemCardSizePriceService = itemCardSizePriceService;
            _itemCardQueries = itemCardQueries;
            _categoryQueries = categoryQueries;
        }

        public async Task UploadThumbnailImageAsync(int id, MemoryStream stream, string fileExtension, CancellationToken cancellationToken)
        {
            var uniqueFileName = Convert.ToString(Guid.NewGuid());
            var directoryPath = Path.Combine(_environment.WebRootPath, ItemCardConstants.thumbnailsMainDirectory, $@"{ItemCardConstants.categorySubdirectoryPrefix}{id}");
            _fileProcessingService.CreateDirectory(directoryPath);
            var image = Image.FromStream(stream);
            var pathForImage = Path.Combine(directoryPath, $@"{uniqueFileName + fileExtension}");
            _fileProcessingService.CreateScaledImage(image, pathForImage, ItemCardConstants.thumbnailImageSize, ItemCardConstants.thumbnailImageSize);

            var itemCardRepository = _unitOfWork.GetRepository<DAL.Model.ItemCard>();
            var itemCardRecord = await itemCardRepository.FindAsync(id).ConfigureAwait(false);
            if (itemCardRecord == null)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(itemCardRecord.ThumbnailImagePath))
            {
                var oldImagePath = Path.Combine(_environment.WebRootPath, itemCardRecord.ThumbnailImagePath);
                _fileProcessingService.DeleteFile(oldImagePath);
            }
            itemCardRecord.ThumbnailImagePath = Path.Combine(ItemCardConstants.thumbnailsMainDirectory, $@"{ItemCardConstants.categorySubdirectoryPrefix}{id}", $@"{uniqueFileName + fileExtension}");
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SynchronizeAdlerItemCardsAsync(IEnumerable<ImportAdlerDto> itemCards, CancellationToken cancellationToken)
        {
            var itemCardRepository = _unitOfWork.GetRepository<DAL.Model.ItemCard>();
            var categories = await _categoryQueries.GetCategoriesCommodityDictionaryAsync(_ajkaShopDbContext, cancellationToken).ConfigureAwait(false);
            foreach (var itemCardImport in itemCards)
            {
                if(!categories.TryGetValue(itemCardImport.CategoryName, out int categoryId))
                {
                    continue;
                }
                var itemCard = await _itemCardQueries.GetEntityAsync(_ajkaShopDbContext, itemCardImport.CategoryId, cancellationToken).ConfigureAwait(false);
                if(itemCard == null)
                {
                    itemCard = UpdateItemCard(new DAL.Model.ItemCard(), itemCardImport, categoryId);
                    await itemCardRepository.InsertAsync(itemCard).ConfigureAwait(false);
                }
                else
                {
                    var itemForUpdate = UpdateItemCard(itemCard, itemCardImport, categoryId);
                    itemCardRepository.Update(itemForUpdate);
                }
                await _unitOfWork.SaveChangesAsync();

                await _itemCardImageService.SynchronizeAdlerImagesAsync(itemCard.Id, itemCardImport, cancellationToken).ConfigureAwait(false);
                await _itemCardSizePriceService.SynchronizeSizePriceVariantsAsync(itemCard.Id, itemCardImport.Sizes, cancellationToken).ConfigureAwait(false);
            }
        }

        private DAL.Model.ItemCard UpdateItemCard(DAL.Model.ItemCard itemCard, ImportAdlerDto itemCardImport, int categoryId)
        {
            itemCard.CategoryId = categoryId;
            itemCard.Headline = $"{itemCardImport.ProductName} {ConvertConstantToDescriptionHelper.ConvertProductLabelToDescription(itemCardImport.ProductLabel)}";         
            itemCard.Description = itemCardImport.Description;
            itemCard.Description += $"<p><b>Gramáž: </b>{itemCardImport.Grammage}</p>";
            itemCard.Description += $"<p><b>Typ:</b>{string.Join(",", itemCardImport.SexList.Select(type => ConvertConstantToDescriptionHelper.ConvertSexTypeToDescription(type)))}</p>";
            itemCard.Quantity = itemCardImport.Quantity;
            itemCard.Price = PriceRoundHelper.RoundToFive(Decimal.Round(itemCardImport.Price * ItemCardConstants.priceMultiplier));
            itemCard.CommodityIdentifier = itemCardImport.CategoryId;
            itemCard.IsAdlerProduct = true;
            itemCard.State = Common.Enums.ItemCardEnums.ItemCardState.ForSale;
            if (itemCard.Quantity <= 0)
            {
                itemCard.State = Common.Enums.ItemCardEnums.ItemCardState.Inactive;
            }
            return itemCard;
        }
    }
}
