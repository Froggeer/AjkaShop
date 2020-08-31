namespace Ajka.Common.Enums
{
    public class InvoiceEnums
    {
        /// <summary>
        /// Payment methods for payment of goods.
        /// </summary>
        public enum InvoicePaymentMethod
        {
            Undefined = 0,

            /// <summary>
            /// Payment is made by bank transfer.
            /// </summary>
            BankTransfer = 1,

            /// <summary>
            /// Payment is made by cash in case of personal delivery.
            /// </summary>
            Cash = 2
        }
    }
}
