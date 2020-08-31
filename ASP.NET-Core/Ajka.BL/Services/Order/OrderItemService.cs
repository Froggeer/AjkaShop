using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.ItemCard;
using Ajka.BL.Models.Order;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.BL.Services.Order.Interfaces;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;

namespace Ajka.BL.Services.Order
{
    public class OrderItemService : IOrderItemService, IAjkaShopService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemService(IUnitOfWork unitOfWork,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<OrderBasketItemDto>> CreateOrderItemsFromBasketAsync(IList<OrderBasketItemDto> basketItems, int orderId, CancellationToken cancellationToken)
        {
            var orderItemRepository = _unitOfWork.GetRepository<DAL.Model.OrderItem>();
            var itemCardRepository = _unitOfWork.GetRepository<DAL.Model.ItemCard>();
            var itemCardSizePriceRepository = _unitOfWork.GetRepository<DAL.Model.ItemCardSizePrice>();
            var approvedItems = new List<OrderBasketItemDto>();
            foreach (var itemInBasket in basketItems)
            {
                var itemCard = await itemCardRepository.FindAsync(itemInBasket.ItemCardId).ConfigureAwait(false);
                if (itemCard != null && itemInBasket.OrderedQuantity > 0 && itemCard.Quantity > 0)
                {
                    if(itemInBasket.OrderedQuantity > itemCard.Quantity)
                    {
                        itemInBasket.OrderedQuantity = itemCard.Quantity;
                    }
                    var orderItem = new DAL.Model.OrderItem
                    {
                        OrderId = orderId,
                        ItemCardId = itemCard.Id,
                        ItemCardSizePriceId = itemInBasket.ItemCardSizePriceId,
                        OrderedQuantity = itemInBasket.OrderedQuantity,
                        ColorName = itemInBasket.ColorName,
                        SizeName = itemInBasket.SizeName,
                        ImagePath = itemInBasket.ImagePath
                    };
                    await orderItemRepository.InsertAsync(orderItem).ConfigureAwait(false);
                    itemInBasket.ItemCard = _mapper.Map<ItemCardDto>(itemCard);
                    if(itemInBasket.ItemCardSizePriceId != null)
                    {
                        var sizePriceRecord = await itemCardSizePriceRepository.FindAsync(itemInBasket.ItemCardSizePriceId).ConfigureAwait(false);
                        if(sizePriceRecord != null)
                        {
                            itemInBasket.ItemCardSizePrice = _mapper.Map<ItemCardSizePriceDto>(sizePriceRecord);
                        }
                    }
                    approvedItems.Add(itemInBasket);
                }
            }
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return approvedItems;
        }
    }
}
