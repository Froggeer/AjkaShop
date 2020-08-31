using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.ProductImport;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.ItemCard;
using Ajka.DAL.Model;
using Ajka.DAL.Model.Interfaces;
using Ajka.Tests.Tools;
using Arch.EntityFrameworkCore.UnitOfWork;
using Moq;
using Xunit;

namespace Ajka.Tests.Services
{
    public class ItemCardSizePriceServiceTest
    {
        private const int someItemCardId = 1;
        private const string notExistingSizeNameInImport = "M";
        private const string someSizeNameImportRecord = "XL";

        [Fact]
        public void SynchronizeSizePriceVariants_Succeeds()
        {
            // Arrange

            var ajkaShopDbContext = new Mock<IAjkaShopDbContext>();
            var repository = new TestRepository<ItemCardSizePrice>
            {
                Repository = ItemCardSizePricesTestData().ToList()
            };

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.GetRepository<ItemCardSizePrice>(It.IsAny<bool>())).Returns(repository);

            var itemCardSizePriceQueries = new Mock<IItemCardSizePriceQueries>();
            itemCardSizePriceQueries.Setup(x => x.GetEntitiesAsync(It.IsAny<IAjkaShopDbContext>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(ItemCardSizePricesTestData()));

            var serviceForTest = new ItemCardSizePriceService(ajkaShopDbContext.Object, unitOfWork.Object, itemCardSizePriceQueries.Object);

            // Act

            serviceForTest.SynchronizeSizePriceVariantsAsync(someItemCardId, ImportSizePricesTestData(), CancellationToken.None).Wait();

            // Assert

            // In imported records is not included size "M", so it must be removed from database.
            Assert.Empty(repository.Repository.Where(x => x.SizeName == notExistingSizeNameInImport));

            // This record have Price updated with computed new value (1000 * 1.3).
            Assert.True(repository.Repository.Where(x => x.SizeName == someSizeNameImportRecord).First().Price > 1000);

            // There still be 3 records in database - 2 which allready exists, one is deleted but new one is imported.
            Assert.Equal(3, repository.Repository.Count);
        }

        private IList<ImportAdlerSizeDto> ImportSizePricesTestData()
        {
            return new List<ImportAdlerSizeDto>
            {
                new ImportAdlerSizeDto
                {
                    SizeName = someSizeNameImportRecord,
                    Price = 1000
                },
                new ImportAdlerSizeDto
                {
                    SizeName = "2XL",
                    Price = 30
                },
                new ImportAdlerSizeDto
                {
                    SizeName = "S",
                    Price = 50
                }
            };
        }

        private IEnumerable<ItemCardSizePrice> ItemCardSizePricesTestData()
        {
            return new List<ItemCardSizePrice>
            {
                new ItemCardSizePrice
                {
                    Id = 1,
                    SizeName = "XL",
                    Price = 10
                },
                new ItemCardSizePrice
                {
                    Id = 2,
                    SizeName = "2XL",
                    Price = 30
                },
                new ItemCardSizePrice
                {
                    Id = 3,
                    SizeName = notExistingSizeNameInImport,
                    Price = 40
                },
            };
        }
    }
}
