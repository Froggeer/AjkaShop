using Ajka.DAL.Model.Interfaces;
using System;
using static Ajka.Common.Enums.InvoiceEnums;

namespace Ajka.BL.Models.Invoice
{
    public class InvoiceDto : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Serial number of the invoice.
        /// </summary>
        public int InvoiceNumber { get; set; }

        /// <summary>
        /// Recipient's address details.
        /// </summary>
        public string RecipientName { get; set; }

        public string RecipientStreet { get; set; }

        public string RecipientCity { get; set; }

        public string RecipientZipCode { get; set; }

        /// <summary>
        /// Variable symbol used in bank payment methods.
        /// </summary>
        public string VariableSymbol { get; set; }

        /// <summary>
        /// How goods will be paid.
        /// </summary>
        public InvoicePaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Date, when this invoice was created.
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// The day of the last payment option.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Date of taxable supply.
        /// </summary>
        public DateTime TaxablePerformanceDate { get; set; }
    }
}
