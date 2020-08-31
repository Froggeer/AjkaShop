using System;
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
using Ajka.DAL.Model;
using Ajka.DAL.Model.Interfaces;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;

namespace Ajka.BL.Services.ItemCard
{
    public class ItemCardImageService : IItemCardImageService, IAjkaShopService
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IItemCardImageQueries _itemCardImageQueries;

        public ItemCardImageService(IAjkaShopDbContext ajkaShopDbContext,
                                    IUnitOfWork unitOfWork,
                                    IWebHostEnvironment environment,
                                    IFileProcessingService fileProcessingService,
                                    IItemCardImageQueries itemCardImageQueries)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _unitOfWork = unitOfWork;
            _environment = environment;
            _fileProcessingService = fileProcessingService;
            _itemCardImageQueries = itemCardImageQueries;
        }

        public async Task UploadImageAsync(int itemCardId, MemoryStream stream, string fileExtension, CancellationToken cancellationToken)
        {
            var itemCardRepository = _unitOfWork.GetRepository<DAL.Model.ItemCard>();
            var checkItemCard = await itemCardRepository.FindAsync(itemCardId).ConfigureAwait(false);
            if(checkItemCard == null)
            {
                return;
            }

            var uniqueFileName = Convert.ToString(Guid.NewGuid());
            var directoryPath = Path.Combine(_environment.WebRootPath, ItemCardConstants.imagesMainDirectory, $@"{ItemCardConstants.itemCardSubdirectoryPrefix}{itemCardId}");
            _fileProcessingService.CreateDirectory(directoryPath);
            var image = Image.FromStream(stream);
            var pathForImage = Path.Combine(directoryPath, $@"{uniqueFileName + fileExtension}");
            _fileProcessingService.CreateScaledImage(image, pathForImage, ItemCardConstants.imageWidthImageSize);

            var itemCardImageRepository = _unitOfWork.GetRepository<ItemCardImage>();
            var recordForAdd = new ItemCardImage
            {
                ItemCardId = itemCardId,
                ImagePath = Path.Combine(ItemCardConstants.imagesMainDirectory, $@"{ItemCardConstants.itemCardSubdirectoryPrefix}{itemCardId}", $@"{uniqueFileName + fileExtension}")
            };
            recordForAdd.ImagePath = recordForAdd.ImagePath.Replace('\\', '/');
            await itemCardImageRepository.InsertAsync(recordForAdd).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteImageAsync(int id, CancellationToken cancellationToken)
        {
            var itemCardImageRepository = _unitOfWork.GetRepository<ItemCardImage>();
            var itemCardImageRecord = await itemCardImageRepository.FindAsync(id).ConfigureAwait(false);
            if (itemCardImageRecord == null)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(itemCardImageRecord.ImagePath))
            {
                var oldImagePath = Path.Combine(_environment.WebRootPath, itemCardImageRecord.ImagePath);
                _fileProcessingService.DeleteFile(oldImagePath);
            }
            itemCardImageRepository.Delete(itemCardImageRecord);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task SynchronizeAdlerImagesAsync(int itemCardId, ImportAdlerDto itemCardImport, CancellationToken cancellationToken)
        {
            var itemCardImageRepository = _unitOfWork.GetRepository<ItemCardImage>();
            var images = await _itemCardImageQueries.GetEntitiesAsync(_ajkaShopDbContext, itemCardId, cancellationToken).ConfigureAwait(false);
            foreach(var imageRecord in images)
            {
                var checkImage = itemCardImport.ImagePaths.FirstOrDefault(s => s.ImagePath == imageRecord.ImagePath);
                if (checkImage != null)
                {
                    imageRecord.ColorName = checkImage.ColorName;
                    imageRecord.AvailableSizesList = checkImage.AvailableSizesList;
                    itemCardImageRepository.Update(imageRecord);
                    itemCardImport.ImagePaths.Remove(checkImage);
                }
            }
            if (itemCardImport.ImagePaths.Any())
            {
                foreach (var image in itemCardImport.ImagePaths)
                {
                    var imageForInsert = new ItemCardImage
                    {
                        ItemCardId = itemCardId,
                        ColorName = image.ColorName,
                        ImagePath = image.ImagePath
                    };
                    await itemCardImageRepository.InsertAsync(imageForInsert).ConfigureAwait(false);
                }
            }
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
