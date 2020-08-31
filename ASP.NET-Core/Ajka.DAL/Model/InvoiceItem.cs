using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.DAL.Model
{
    [Table(nameof(InvoiceItem))]
    public class InvoiceItem : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Link to the invoice table.
        /// </summary>
        public virtual Invoice Invoice { get; set; }

        public int InvoiceId { get; set; }

        /// <summary>
        /// Position of this line in invoice (ascending from top).
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Text description of the goods.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price in CZK for one piece of goods.
        /// </summary>
        public decimal PricePerPiece { get; set; }

        /// <summary>
        /// Quantity of goods pieces.
        /// </summary>
        public int Quantity { get; set; }
    }
}
