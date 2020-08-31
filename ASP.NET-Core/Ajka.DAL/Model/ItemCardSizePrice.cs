using System.ComponentModel.DataAnnotations.Schema;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.DAL.Model
{
    /// <summary>
    /// Variation of prices for specific sizes od product.
    /// </summary>
    [Table(nameof(ItemCardSizePrice))]
    public class ItemCardSizePrice : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Link to the item card table.
        /// </summary>
        public virtual ItemCard ItemCard { get; set; }

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
