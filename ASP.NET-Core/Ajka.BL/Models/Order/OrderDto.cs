using System;
using Ajka.DAL.Model.Interfaces;
using static Ajka.Common.Enums.OrderEnums;

namespace Ajka.BL.Models.Order
{
    /// <summary>
    /// Customers order.
    /// </summary>
    public class OrderDto : IEntity<int>
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Custom discount, ordered by administrator.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Email of customer, where administrator will send payment details.
        /// </summary>
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Note from customer.
        /// </summary>
        public string Note { get; set; }

        public OrderState State { get; set; }
    }
}
