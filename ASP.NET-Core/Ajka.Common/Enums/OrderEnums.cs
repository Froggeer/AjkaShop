namespace Ajka.Common.Enums
{
    public class OrderEnums
    {
        /// <summary>
        /// Goods sales record statuses.
        /// </summary>
        public enum OrderState
        {
            Undefined = 0,

            /// <summary>
            /// Order is just created by customer and waits for administrator to process.
            /// </summary>
            Created = 1,

            /// <summary>
            /// The customer has received payment instructions and is awaiting payment.
            /// </summary>
            WaitingForPayment = 2,

            /// <summary>
            /// Order is successfully completed, bought goods has removed from offer.
            /// </summary>
            Closed = 3,

            /// <summary>
            /// Order is cancelled and nothing change.
            /// </summary>
            Cancellation = 4
        }
    }
}
