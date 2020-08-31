using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.ProductImport;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.BL.Services.ItemCard;
using Ajka.BL.Services.ItemCard.Interfaces;
using Ajka.DAL.Model.Interfaces;
using Ajka.Tests.Tools;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace Ajka.Tests.Services
{
    public class ItemCardServiceTest
    {
        private const int idOfItemCard = 1;
        private const string commodityIdentifier = "256";

        private readonly TestRepository<DAL.Model.ItemCard> repository = new TestRepository<DAL.Model.ItemCard>();
        private readonly Mock<IAjkaShopDbContext> ajkaShopDbContext = new Mock<IAjkaShopDbContext>();
        private readonly Mock<IUnitOfWork> unitOfWork = new Mock<IUnitOfWork>();
        private readonly Mock<IWebHostEnvironment> environment = new Mock<IWebHostEnvironment>();
        private readonly Mock<IFileProcessingService> fileProcessingService = new Mock<IFileProcessingService>();
        private readonly Mock<IItemCardImageService> itemCardImageService = new Mock<IItemCardImageService>();
        private readonly Mock<IItemCardSizePriceService> itemCardSizePriceService = new Mock<IItemCardSizePriceService>();
        private readonly Mock<IItemCardQueries> itemCardQueries = new Mock<IItemCardQueries>();
        private readonly Mock<ICategoryQueries> categoryQueries = new Mock<ICategoryQueries>();

        [Fact]
        public void UploadThumbnailImage_Succeeds()
        {
            // Arrange

            repository.Repository.Add(new DAL.Model.ItemCard
            {
                Id = idOfItemCard
            });

            var memoryStream = new MemoryStream();
            var testImage = new Bitmap(2, 2);
            testImage.Save(memoryStream, ImageFormat.Jpeg);

            unitOfWork.Setup(x => x.GetRepository<DAL.Model.ItemCard>(It.IsAny<bool>())).Returns(repository);
            environment.Setup(x => x.WebRootPath).Returns(Guid.NewGuid().ToString());

            var serviceForTest = new ItemCardService(ajkaShopDbContext.Object, unitOfWork.Object, environment.Object, fileProcessingService.Object, 
                itemCardImageService.Object, itemCardSizePriceService.Object, itemCardQueries.Object, categoryQueries.Object);

            // Act

            serviceForTest.UploadThumbnailImageAsync(idOfItemCard, memoryStream, Guid.NewGuid().ToString(), CancellationToken.None).Wait();

            // Assert

            // Thumbnail image is created and network path to this file is stored in DB.
            Assert.NotNull(repository.Repository.Where(x => x.ThumbnailImagePath != null));
        }

        [Fact]
        public void SynchronizeAdlerItemCards_Succeeds()
        {
            // Arrange

            repository.Repository.Add(new DAL.Model.ItemCard
            {
                Id = idOfItemCard,
                CommodityIdentifier = commodityIdentifier
            });
            unitOfWork.Setup(x => x.GetRepository<DAL.Model.ItemCard>(It.IsAny<bool>())).Returns(repository);

            var categories = new Dictionary<string, int>
            {
                { "TRICKA", 1004 },
                { "POLOKOSILE", 1005 },
                { "KOSILE", 1006 }
            };
            categoryQueries.Setup(x => x.GetCategoriesCommodityDictionaryAsync(It.IsAny<IAjkaShopDbContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((IDictionary<string, int>)categories));

            itemCardQueries.Setup(x => x.GetEntityAsync(It.IsAny<IAjkaShopDbContext>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns<IAjkaShopDbContext, string, CancellationToken>((context, categoryId, token) =>
                {
                    if (categoryId == commodityIdentifier)
                    {
                        return Task.FromResult(new DAL.Model.ItemCard
                        {
                            Id = idOfItemCard,
                            CommodityIdentifier = commodityIdentifier
                        });
                    }
                    return Task.FromResult((DAL.Model.ItemCard) null);
                });

            var serviceForTest = new ItemCardService(ajkaShopDbContext.Object, unitOfWork.Object, environment.Object, fileProcessingService.Object, 
                itemCardImageService.Object, itemCardSizePriceService.Object, itemCardQueries.Object, categoryQueries.Object);

            // Act

            serviceForTest.SynchronizeAdlerItemCardsAsync(ImportAdlerTestData(), CancellationToken.None).Wait();

            // Assert

            // There is one new record and one existing (updated) record.
            Assert.Equal(2, repository.Repository.Count);

            // Check for update of existing record on two random chosen attributes.
            Assert.NotEmpty(repository.Repository.Where(x => x.CommodityIdentifier == commodityIdentifier && x.Description != null && x.Quantity > 0));

            // Because all item cards have quantity higher than zero, both records is available for sale.
            Assert.Empty(repository.Repository.Where(x => x.State != Common.Enums.ItemCardEnums.ItemCardState.ForSale));
        }

        private IEnumerable<ImportAdlerDto> ImportAdlerTestData()
        {
            return new List<ImportAdlerDto>
            {
                new ImportAdlerDto
                {
                    CategoryId = "C36",
                    CategoryName = "TRICKA",
                    ProductName = "Camo Triumph",
                    ProductLabel = "MALFINI",
                    Description = "<ul><li>strečový materiál udržující stálost tvaru</li></ul>",
                    SizeName = "XS",
                    ColorName = null,
                    SizeId = null,
                    Price = 106.90m,
                    Quantity = 200,
                    ProductImagePath = null,
                    Grammage = "180 g/m² ",
                    SexList = new List<string>{ "LADIES", "GENTS" },
                    ImagePaths = new List<ImportAdlerImageDto>
                    {
                        new ImportAdlerImageDto { ImagePath = "https://share.adler.info/images/product/C36/C36_32_C_xl.jpg" },
                        new ImportAdlerImageDto { ImagePath = "https://share.adler.info/images/product/C36/C36_33_C_xl.jpg" },
                        new ImportAdlerImageDto { ImagePath = "https://share.adler.info/images/product/C36/C36_34_C_xl.jpg" }
                    },
                    Sizes = new List<ImportAdlerSizeDto>
                    {
                        new ImportAdlerSizeDto
                        {
                            SizeId ="12",
                            SizeName = "XS"
                        },
                        new ImportAdlerSizeDto{
                            SizeId ="13",
                            SizeName = "S"
                        },
                        new ImportAdlerSizeDto
                        {
                            SizeId ="14",
                            SizeName = "M"
                        }
                    }
                },
                new ImportAdlerDto
                {
                    CategoryId = commodityIdentifier,
                    CategoryName = "POLOKOSILE",
                    ProductName = "Grand",
                    ProductLabel = "MALFINIPREMIUM",
                    Description = "<ul><li>strečový materiál udržující stálost tvaru</li></ul>",
                    SizeName = "3XL",
                    ColorName = null,
                    SizeId = null,
                    Price = 106.90m,
                    Quantity = 200,
                    ProductImagePath = null,
                    Grammage = "160 g/m² ",
                    SexList = new List<string>{ "LADIES" },
                    ImagePaths = new List<ImportAdlerImageDto>
                    {
                        new ImportAdlerImageDto { ImagePath = "https://share.adler.info/images/product/259/259_00_C_xl.jpg" },
                        new ImportAdlerImageDto { ImagePath = "https://share.adler.info/images/product/259/259_01_C_xl.jpg" },
                        new ImportAdlerImageDto { ImagePath = "https://share.adler.info/images/product/259/259_02_C_xl.jpg" },
                        new ImportAdlerImageDto { ImagePath = "https://share.adler.info/images/product/259/259_71_C_xl.jpg" }
                    },
                    Sizes = new List<ImportAdlerSizeDto>
                    {
                        new ImportAdlerSizeDto
                        {
                            SizeId ="16",
                            SizeName = "XL"
                        },
                        new ImportAdlerSizeDto{
                            SizeId ="17",
                            SizeName = "2XL"
                        },
                        new ImportAdlerSizeDto
                        {
                            SizeId ="18",
                            SizeName = "3XL"
                        }
                    }
                }
            };
        }
    }
}
