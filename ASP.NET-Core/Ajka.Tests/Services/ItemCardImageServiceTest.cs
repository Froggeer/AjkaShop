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
using Ajka.DAL.Model;
using Ajka.DAL.Model.Interfaces;
using Ajka.Tests.Tools;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace Ajka.Tests.Services
{
    public class ItemCardImageServiceTest
    {
        private const int idOfItemCard = 1;
        private const int idOfAnotherItemCard = 2;
        private const int idOfItemCardImage = 3;
        private const string someImagePath = "1346789ABCD";

        private readonly TestRepository<ItemCard> itemCardRepository = new TestRepository<ItemCard>();
        private readonly TestRepository<ItemCardImage> itemCardImageRepository = new TestRepository<ItemCardImage>();
        private readonly Mock<IAjkaShopDbContext> ajkaShopDbContext = new Mock<IAjkaShopDbContext>();
        private readonly Mock<IUnitOfWork> unitOfWork = new Mock<IUnitOfWork>();
        private readonly Mock<IWebHostEnvironment> environment = new Mock<IWebHostEnvironment>();
        private readonly Mock<IFileProcessingService> fileProcessingService = new Mock<IFileProcessingService>();
        private readonly Mock<IItemCardImageQueries> itemCardImageQueries = new Mock<IItemCardImageQueries>();

        [Fact]
        public void UploadImage_Fails()
        {
            // Arrange

            itemCardRepository.Repository.Add(new ItemCard
            {
                Id = idOfItemCard
            });
            itemCardImageRepository.Repository.Add(new ItemCardImage
            {
                Id = idOfItemCardImage
            });

            var memoryStream = new MemoryStream();

            unitOfWork.Setup(x => x.GetRepository<ItemCard>(It.IsAny<bool>())).Returns(itemCardRepository);
            unitOfWork.Setup(x => x.GetRepository<ItemCardImage>(It.IsAny<bool>())).Returns(itemCardImageRepository);

            var serviceForTest = new ItemCardImageService(ajkaShopDbContext.Object, unitOfWork.Object, environment.Object, fileProcessingService.Object, itemCardImageQueries.Object);

            // Act

            serviceForTest.UploadImageAsync(idOfAnotherItemCard, memoryStream, Guid.NewGuid().ToString(), CancellationToken.None).Wait();

            // Assert

            // Image is not created, because there is not DB record of item card.
            Assert.NotNull(itemCardImageRepository.Repository.Where(x => x.ImagePath == null));
        }

        [Fact]
        public void UploadImage_Succeeds()
        {
            // Arrange

            itemCardRepository.Repository.Add(new ItemCard
            {
                Id = idOfItemCard
            });
            itemCardImageRepository.Repository.Add(new ItemCardImage
            {
                Id = idOfItemCardImage
            });

            var memoryStream = new MemoryStream();
            var testImage = new Bitmap(2, 2);
            testImage.Save(memoryStream, ImageFormat.Jpeg);

            unitOfWork.Setup(x => x.GetRepository<ItemCard>(It.IsAny<bool>())).Returns(itemCardRepository);
            unitOfWork.Setup(x => x.GetRepository<ItemCardImage>(It.IsAny<bool>())).Returns(itemCardImageRepository);

            environment.Setup(x => x.WebRootPath).Returns(Guid.NewGuid().ToString());

            var serviceForTest = new ItemCardImageService(ajkaShopDbContext.Object, unitOfWork.Object, environment.Object, fileProcessingService.Object, itemCardImageQueries.Object);

            // Act

            serviceForTest.UploadImageAsync(idOfItemCard, memoryStream, Guid.NewGuid().ToString(), CancellationToken.None).Wait();

            // Assert

            // Image is succesfully created and path is registered in DB.
            Assert.NotNull(itemCardImageRepository.Repository.Where(x => x.ImagePath != null && x.ItemCardId == idOfItemCard));
        }

        [Fact]
        public void SynchronizeAdlerImages_Succeeds()
        {
            // Arrange

            itemCardImageRepository.Repository.Add(GetItemCardImageTestData().First());
            unitOfWork.Setup(x => x.GetRepository<ItemCardImage>(It.IsAny<bool>())).Returns(itemCardImageRepository);

            var someItemImport = new ImportAdlerDto();
            someItemImport.ImagePaths.Add(new ImportAdlerImageDto { ImagePath = someImagePath });
            someItemImport.ImagePaths.Add(new ImportAdlerImageDto { ImagePath = Guid.NewGuid().ToString() });
            someItemImport.ImagePaths.Add(new ImportAdlerImageDto { ImagePath = Guid.NewGuid().ToString() });

            itemCardImageQueries.Setup(x => x.GetEntitiesAsync(It.IsAny<IAjkaShopDbContext>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetItemCardImageTestData()));

            var serviceForTest = new ItemCardImageService(ajkaShopDbContext.Object, unitOfWork.Object, environment.Object, fileProcessingService.Object, itemCardImageQueries.Object);

            // Act

            serviceForTest.SynchronizeAdlerImagesAsync(idOfItemCard, someItemImport, CancellationToken.None).Wait();

            // Assert

            // In repository is one existing record and two new.
            Assert.Equal(3, itemCardImageRepository.Repository.Count);
        }

        private IEnumerable<ItemCardImage> GetItemCardImageTestData()
        {
            return new List<ItemCardImage>
            {
                new ItemCardImage
                {
                    Id = idOfItemCardImage,
                    ItemCardId = idOfItemCard,
                    ImagePath = someImagePath
                }
            };
        }
    }
}
