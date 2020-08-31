using System.Collections.Generic;

namespace Ajka.BL.Models.Order
{
    public class OrderBasketDto
    {
        /// <summary>
        /// Customer email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Note from customer.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// If true, copy of order summary is send to customer email.
        /// </summary>
        public bool IsRequestedCopyOfOrder { get; set; }

        public IList<OrderBasketItemDto> Items { get; set; } = new List<OrderBasketItemDto>();
    }
}
