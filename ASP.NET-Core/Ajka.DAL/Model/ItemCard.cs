using System.ComponentModel.DataAnnotations.Schema;
using Ajka.DAL.Model.Interfaces;
using static Ajka.Common.Enums.ItemCardEnums;

namespace Ajka.DAL.Model
{
    /// <summary>
    /// Preview of goods.
    /// </summary>
    [Table(nameof(ItemCard))]
    public class ItemCard : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Link to the category table.
        /// </summary>
        public virtual Category Category { get; set; }
        
        public int CategoryId { get; set; }

        /// <summary>
        /// Title of good.
        /// </summary>
        public string Headline { get; set; }

        /// <summary>
        /// Detailed description of good (HTML format).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Relative network path to preview image.
        /// </summary>
        public string ThumbnailImagePath { get; set; }

        /// <summary>
        /// Available quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Price for one item of goods in CZK.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Text identification of commodity type, used in import Adler products function.
        /// </summary>
        public string CommodityIdentifier { get; set; }

        /// <summary>
        /// If true, then this product is imported from Adler web service.
        /// </summary>
        public bool IsAdlerProduct { get; set; }

        /// <summary>
        /// States of item in selling process.
        /// </summary>
        public ItemCardState State { get; set; }
    }
}
