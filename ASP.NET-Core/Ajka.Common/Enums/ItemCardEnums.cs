namespace Ajka.Common.Enums
{
    public class ItemCardEnums
    {
        /// <summary>
        /// Goods sales record statuses.
        /// </summary>
        public enum ItemCardState
        {
            Undefined = 0,

            /// <summary>
            /// The goods is available for sale to customers.
            /// </summary>
            ForSale = 1,

            /// <summary>
            /// The goods is sold and will be never offered for sale again.
            /// </summary>
            Sold = 2,

            /// <summary>
            /// Item is hidden from eshop offer, but can be anytime returned to sale.
            /// </summary>
            Inactive = 3
        }
    }
}
