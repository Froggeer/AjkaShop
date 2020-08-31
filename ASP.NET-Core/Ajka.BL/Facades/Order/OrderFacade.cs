using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.Order;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.BL.Services.Order.Interfaces;
using Ajka.BL.Facades.Order.Interfaces;
using Arch.EntityFrameworkCore.UnitOfWork;
using System.Text.RegularExpressions;
using Ajka.Common.Constants.Service;

namespace Ajka.BL.Facades.Order
{
    public class OrderFacade : IOrderFacade, IAjkaShopService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderEmailService _orderEmailService;
        private readonly IErrorLogService _errorLogService;
        private readonly IOrderItemService _orderItemService;

        public OrderFacade(IUnitOfWork unitOfWork, 
                           IOrderEmailService orderEmailService,
                           IErrorLogService errorLogService,
                           IOrderItemService orderItemService)
        {
            _unitOfWork = unitOfWork;
            _orderEmailService = orderEmailService;
            _errorLogService = errorLogService;
            _orderItemService = orderItemService;
        }

        public async Task<string> CreateOrderAsync(OrderBasketDto basket, CancellationToken cancellationToken)
        {
            var isEmail = Regex.IsMatch(basket.Email, OrderEmailConstants.emailFormatMatchRegex, RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                return OrderEmailConstants.errorEmailAddressIsNotValid;
            }

            try
            {
                var orderRepository = _unitOfWork.GetRepository<DAL.Model.Order>();
                var newOrder = new DAL.Model.Order
                {
                    CreateDate = DateTime.Now,
                    CustomerEmail = basket.Email,
                    Note = basket.Note,
                    State = Common.Enums.OrderEnums.OrderState.Created
                };
                await orderRepository.InsertAsync(newOrder).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                var itemCardsInBasket = await _orderItemService.CreateOrderItemsFromBasketAsync(basket.Items, newOrder.Id, cancellationToken).ConfigureAwait(false);
                if (!itemCardsInBasket.Any())
                {
                    return OrderEmailConstants.errorNoItemsInBasket;
                }

                var emailBody = await _orderEmailService.RenderViewToStringAsync(itemCardsInBasket, basket.Email, newOrder.Id.ToString(), cancellationToken).ConfigureAwait(false);
                await _orderEmailService.SendAsync(basket.Email, emailBody, basket.IsRequestedCopyOfOrder, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogExceptionAsync(ex).ConfigureAwait(false);
                return $"Systémová chyba: {ex.Message}";
            }
            return string.Empty;
        }
    }
}
