using System.ComponentModel.DataAnnotations.Schema;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.DAL.Model
{
    [Table(nameof(OrderItem))]
    public class OrderItem : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Link to the order table.
        /// </summary>
        public virtual Order Order { get; set; }

        public int OrderId { get; set; }

        /// <summary>
        /// Link to the item goods table.
        /// </summary>
        public virtual ItemCard ItemCard { get; set; }

        public int ItemCardId { get; set; }

        /// <summary>
        /// Link to the item card table.
        /// </summary>
        public virtual ItemCardSizePrice ItemCardSizePrice { get; set; }

        public int? ItemCardSizePriceId { get; set; }

        /// <summary>
        /// Quantity of goods, which customer wants.
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
        /// Relative network path to image.
        /// </summary>
        public string ImagePath { get; set; }
    }
}
