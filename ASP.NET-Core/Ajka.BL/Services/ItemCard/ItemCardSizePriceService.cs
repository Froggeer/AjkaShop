using Ajka.BL.Models.ProductImport;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.BL.Services.ItemCard.Interfaces;
using Ajka.Common.Constants.Service;
using Ajka.Common.Helpers;
using Ajka.DAL.Model;
using Ajka.DAL.Model.Interfaces;
using Arch.EntityFrameworkCore.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Services.ItemCard
{
    public class ItemCardSizePriceService : IItemCardSizePriceService, IAjkaShopService
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemCardSizePriceQueries _itemCardSizePriceQueries;

        public ItemCardSizePriceService(IAjkaShopDbContext ajkaShopDbContext,
                                        IUnitOfWork unitOfWork,
                                        IItemCardSizePriceQueries itemCardSizePriceQueries)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _unitOfWork = unitOfWork;
            _itemCardSizePriceQueries = itemCardSizePriceQueries;
        }

        public async Task SynchronizeSizePriceVariantsAsync(int itemCardId, IList<ImportAdlerSizeDto> importSizePrices, CancellationToken cancellationToken)
        {
            var sizePrices = await _itemCardSizePriceQueries.GetEntitiesAsync(_ajkaShopDbContext, itemCardId, cancellationToken).ConfigureAwait(false);
            var itemCardSizePriceRepository = _unitOfWork.GetRepository<ItemCardSizePrice>();
            foreach (var sizePriceRecord in sizePrices)
            {
                var checkImportRecord = importSizePrices.Where(x => x.SizeName == sizePriceRecord.SizeName).FirstOrDefault();
                if (checkImportRecord == null)
                {
                    itemCardSizePriceRepository.Delete(sizePriceRecord.Id);
                }
                else
                {
                    sizePriceRecord.Price = PriceRoundHelper.RoundToFive(Decimal.Round(checkImportRecord.Price * ItemCardConstants.priceMultiplier));
                    itemCardSizePriceRepository.Update(sizePriceRecord);
                    importSizePrices.Remove(checkImportRecord);
                }
            }
            foreach (var importRecord in importSizePrices)
            {
                var newRecord = new ItemCardSizePrice
                {
                    ItemCardId = itemCardId,
                    SizeName = importRecord.SizeName,
                    Price = PriceRoundHelper.RoundToFive(Decimal.Round(importRecord.Price * ItemCardConstants.priceMultiplier))
                };
                await itemCardSizePriceRepository.InsertAsync(newRecord).ConfigureAwait(false);
            }
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
