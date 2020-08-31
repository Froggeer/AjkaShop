using Ajka.DAL.Model.Interfaces;

namespace Ajka.BL.Models.ItemCard
{
    /// <summary>
    /// Variation of prices for specific sizes od product.
    /// </summary>
    public class ItemCardSizePriceDto : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Link to the item card table.
        /// </summary>
        public int ItemCardId { get; set; }

        /// <summary>
        /// Short name for size (identifier).
        /// </summary>
        public string SizeName { get; set; }

        /// <summary>
        /// Price for one item of goods in CZK.
        /// </summary>
        public decimal Price { get; set; }
    }
}
