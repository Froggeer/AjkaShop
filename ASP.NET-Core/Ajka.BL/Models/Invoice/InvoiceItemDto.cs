using Ajka.DAL.Model.Interfaces;

namespace Ajka.BL.Models.Invoice
{
    public class InvoiceItemDto : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Link to the invoice table.
        /// </summary>
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
