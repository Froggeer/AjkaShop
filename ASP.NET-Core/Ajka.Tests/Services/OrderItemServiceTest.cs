using System.Collections.Generic;
using System.Threading;
using Ajka.BL.Models.ItemCard;
using Ajka.BL.Models.Order;
using Ajka.BL.Services.Order;
using Ajka.DAL.Model;
using Ajka.Tests.Tools;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Moq;
using Xunit;

namespace Ajka.Tests.Services
{
    public class OrderItemServiceTest
    {
        private const int someOrderId = 1;

        [Fact]
        public void CreateOrderItemsFromBasket_Succeeds()
        {
            // Arrange

            var repository = new TestRepository<ItemCard>
            {
                Repository = ItemCardsTestData()
            };
            var orderItemRepository = new TestRepository<OrderItem>();

            var _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(x => x.GetRepository<OrderItem>(It.IsAny<bool>())).Returns(orderItemRepository);
            _unitOfWork.Setup(x => x.GetRepository<ItemCard>(It.IsAny<bool>())).Returns(repository);

            var _mapper = new Mock<IMapper>();
            _mapper.Setup(x => x.Map<ItemCardDto>(It.IsAny<ItemCard>())).Returns<ItemCard>(r => new ItemCardDto
            {
                Id = r.Id,
                Quantity = r.Quantity
            });

            var serviceForTest = new OrderItemService(_unitOfWork.Object, _mapper.Object);

            // Act

            var result = serviceForTest.CreateOrderItemsFromBasketAsync(BasketItemsTestData(), someOrderId, CancellationToken.None).Result;

            // Assert

            // There are two items recorded, because item with ItemCardId=1 have more ordered quantity as available quantity, 
            // but this quantity can be lowered by service, and item with ItemCardId=3 is fully valid.
            // Item with ItemCardId=2 have 0 ordered quantity (then cant be oredered) and item with ItemCardId=5 does not exists in DB.
            Assert.Equal(2, orderItemRepository.Repository.Count);

            // Size of this collection must be same as number of created OrderItem records.
            Assert.Equal(2, result.Count);
        }

        private IList<OrderBasketItemDto> BasketItemsTestData()
        {
            return new List<OrderBasketItemDto>
            {
                new OrderBasketItemDto
                {
                    ItemCardId = 1,
                    OrderedQuantity = 5
                },
                new OrderBasketItemDto
                {
                    ItemCardId = 2,
                    OrderedQuantity = 0
                },
                new OrderBasketItemDto
                {
                    ItemCardId = 3,
                    OrderedQuantity = 2
                },
                new OrderBasketItemDto
                {
                    ItemCardId = 5,
                    OrderedQuantity = 5
                }
            };
        }

        private List<ItemCard> ItemCardsTestData()
        {
            return new List<ItemCard>
            {
                new ItemCard
                {
                    Id = 1,
                    Quantity = 3
                },
                new ItemCard
                {
                    Id = 2,
                    Quantity = 3
                },
                new ItemCard
                {
                    Id = 3,
                    Quantity = 3
                },
            };
        }
    }
}
