using Ajka.BL.Models.ItemCard;
using System.Collections.Generic;

namespace Ajka.BL.Models.Order
{
    public class OrderEmailSummaryModel
    {
        public string OrderNumber { get; set; }

        public string Email { get; set; }

        public IList<OrderBasketItemDto> OrderBasketItems { get; set; }
    }
}
