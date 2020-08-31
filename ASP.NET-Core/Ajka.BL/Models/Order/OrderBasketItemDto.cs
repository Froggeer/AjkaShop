using Ajka.BL.Models.ItemCard;

namespace Ajka.BL.Models.Order
{
    public class OrderBasketItemDto
    {
        public int ItemCardId { get; set; }

        public virtual ItemCardDto ItemCard { get; set; }

        public int? ItemCardSizePriceId { get; set; }

        public virtual ItemCardSizePriceDto ItemCardSizePrice { get; set; }

        /// <summary>
        /// Quantity of goods, requested by customer.
        /// </summary>
        public int OrderedQuantity { get; set; }

        /// <summary>
        /// Color selected by the customer.
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// Size selected by the customer.
        /// </summary>
        public string SizeName { get; set; }

        /// <summary>
        /// Web path to image of requested goods.
        /// </summary>
        public string ImagePath { get; set; }
    }
}
